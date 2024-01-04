using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Dynamic;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using YBS2.Data.Enums;
using YBS2.Data.Models;
using YBS2.Data.UnitOfWork;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.PageResponses;
using YBS2.Service.Exceptions;

namespace YBS2.Service.Services.Implements
{
    public class VNPayService : IVNPayService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        public VNPayService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        private SortedList<string, string> AddRegisterRequestData(MembershipPackage membershipPackage, Guid memberId, HttpContext context)
        {
            SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
            var callBackUrl = "https://" + context.Request.Host + _configuration["PaymentCallBack:MembershipPaymentReturnUrl"];
            //add basic parameter to VNPay 
            _requestData = AddRequestData("vnp_Version", _configuration["Vnpay:Version"], _requestData);
            _requestData = AddRequestData("vnp_Command", _configuration["Vnpay:Command"], _requestData);
            _requestData = AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"], _requestData);
            _requestData = AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"], _requestData);
            _requestData = AddRequestData("vnp_CurrCode", "VND", _requestData);
            _requestData = AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString() + "," + memberId.ToString(), _requestData);
            _requestData = AddRequestData("vnp_OrderInfo", membershipPackage.Name.ToString() + "," + membershipPackage.Id, _requestData);
            _requestData = AddRequestData("vnp_OrderType", nameof(EnumTransactionType.Booking), _requestData);
            _requestData = AddRequestData("vnp_Amount", ((int)membershipPackage.Price * 100).ToString(), _requestData);
            _requestData = AddRequestData("vnp_ReturnUrl", callBackUrl, _requestData);
            _requestData = AddRequestData("vnp_IpAddr", GetIpAddress(), _requestData);
            _requestData = AddRequestData("vnp_CreateDate", DateTime.UtcNow.AddHours(7).ToString("yyyyMMddHHmmss"), _requestData);
            _requestData = AddRequestData("vnp_ExpireDate", DateTime.UtcNow.AddHours(7).AddMinutes(15).ToString("yyyyMMddHHmmss"), _requestData);
            return _requestData;
        }

        public async Task<string?> CreateRegisterRequestURL(Guid membershipPackageId, Guid memberId, HttpContext context)
        {
            string baseUrl = _configuration["VnPay:BaseUrl"];
            string hashSecret = _configuration["VnPay:HashSecret"];
            MembershipPackage membershipPackage = await _unitOfWork.MembershipPackageRepository
                .GetByID(membershipPackageId);
            if (membershipPackage == null)
            {
                dynamic errors = new ExpandoObject();
                errors.Id = $"Membership package with id {membershipPackageId} does not exist.";
                throw new APIException(HttpStatusCode.BadRequest, errors.Id, errors);
            }
            SortedList<string, string> _requestData = AddRegisterRequestData(membershipPackage, memberId, context);
            var data = new StringBuilder();


            foreach (var (key, value) in _requestData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }


            var querystring = data.ToString();


            baseUrl += "?" + querystring;
            var signData = querystring;
            if (signData.Length > 0)
            {
                signData = signData.Remove(data.Length - 1, 1);
            }


            var vnpSecureHash = HmacSha512(hashSecret, signData);
            baseUrl += "vnp_SecureHash=" + vnpSecureHash;


            return baseUrl;
        }

        public async Task<VNPayRegisterResponse> CallBackRegisterPayment(IQueryCollection collections)
        {
            SortedList<string, string> _responseData = new SortedList<string, string>(new VnPayCompare());
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    _responseData = AddResponseData(key, value, _responseData);
                }
            }
            string[] txnRef = GetResponseData("vnp_TxnRef", _responseData).Trim().Split(",");
            string[] orderInfo = GetResponseData("vnp_OrderInfo", _responseData).Trim().Split(",");
            Guid memberId = Guid.Parse(txnRef[1]);
            Guid membershipPackageId = Guid.Parse(orderInfo[1]);
            string code = txnRef[0];
            float totalAmount = float.Parse(GetResponseData("vnp_Amount", _responseData).Trim()) / 100;
            string name = orderInfo[0];
            string bankCode = GetResponseData("vnp_BankCode", _responseData).Trim();
            string bankTranNo = GetResponseData("vnp_BankTranNo", _responseData).Trim();
            string cardType = GetResponseData("vnp_CardType", _responseData).Trim();
            DateTime paymentDate = DateTime.ParseExact(GetResponseData("vnp_PayDate", _responseData).Trim(), "yyyyMMddHHmmss", null);
            string VNPayCode = GetResponseData("vnp_TransactionNo", _responseData).Trim();
            string responseCode = GetResponseData("vnp_ResponseCode", _responseData).Trim();
            string transactionStatus = GetResponseData("vnp_TransactionStatus", _responseData).Trim();
            string vnpSecureHash =
                collections.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value;

            string hashSecret = _configuration["VnPay:HashSecret"];
            var checkSignature =
                ValidateSignature(vnpSecureHash, hashSecret, _responseData); //check Signature
            bool success = true;

            MembershipPackage? existingMembershipPackage = await _unitOfWork.MembershipPackageRepository
                .Find(membershipPackage => membershipPackage.Id == membershipPackageId)
                .FirstOrDefaultAsync();
            if (existingMembershipPackage == null)
            {
                dynamic errors = new ExpandoObject();
                errors.MembershipPackage = "Membership Package Not Found";
                throw new APIException(HttpStatusCode.BadRequest, errors.MembershipPackage, errors);
            }

            if (!checkSignature)
            {
                dynamic errors = new ExpandoObject();
                errors.Id = $"Invalid Signature";
                throw new APIException(HttpStatusCode.BadRequest, errors.Id, errors);
            }
            if (responseCode != "00" || transactionStatus != "00")
            {
                dynamic errors = new ExpandoObject();
                errors.Id = "Payment Error";
                throw new APIException(HttpStatusCode.BadRequest, errors.Id, errors);
            }
            if (totalAmount != existingMembershipPackage.Price)
            {
                dynamic errors = new ExpandoObject();
                errors.Amount = "Invalid Amount";
                throw new APIException(HttpStatusCode.BadRequest, errors.Amount, errors);
            }
            
            return new VNPayRegisterResponse
            {
                MemberId = memberId,
                MembershipPackageId = membershipPackageId,
                Code = code,
                TotalAmount = totalAmount,
                BankCode = bankCode,
                BankTranNo = bankTranNo,
                CardType = cardType,
                Name = name,
                PaymentDate = paymentDate,
                Success = success,
                VNPayCode = VNPayCode
            };
        }

        private SortedList<string, string> AddBookingRequestData(Booking booking, HttpContext context)
        {
            SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
            var callBackUrl = "https://" + context.Request.Host + _configuration["PaymentCallBack:MembershipPaymentReturnUrl"];
            //add basic parameter to VNPay 
            _requestData = AddRequestData("vnp_Version", _configuration["Vnpay:Version"], _requestData);
            _requestData = AddRequestData("vnp_Command", _configuration["Vnpay:Command"], _requestData);
            _requestData = AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"], _requestData);
            _requestData = AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"], _requestData);
            _requestData = AddRequestData("vnp_CurrCode", "VND", _requestData);
            _requestData = AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString(), _requestData);
            _requestData = AddRequestData("vnp_OrderInfo", booking.Tour.Name.ToString(), _requestData);
            _requestData = AddRequestData("vnp_OrderType", nameof(EnumTransactionType.Booking), _requestData);
            _requestData = AddRequestData("vnp_Amount", ((int)booking.TotalAmount * 100).ToString(), _requestData);
            _requestData = AddRequestData("vnp_ReturnUrl", callBackUrl, _requestData);
            _requestData = AddRequestData("vnp_IpAddr", GetIpAddress(), _requestData);
            _requestData = AddRequestData("vnp_CreateDate", DateTime.UtcNow.AddHours(7).ToString("yyyyMMddHHmmss"), _requestData);
            _requestData = AddRequestData("vnp_ExpireDate", DateTime.UtcNow.AddHours(7).AddMinutes(15).ToString("yyyyMMddHHmmss"), _requestData);
            return _requestData;
        }

        public async Task<string> CreateBookingRequestURL(Guid bookingId, HttpContext context)
        {
            string baseUrl = _configuration["VnPay:BaseUrl"];
            string hashSecret = _configuration["VnPay:HashSecret"];
            Booking? booking = await _unitOfWork.BookingRepository
                .Find(booking => booking.Id == bookingId)
                .Include(booking => booking.Tour)
                .FirstOrDefaultAsync();
            if (booking == null)
            {
                dynamic errors = new ExpandoObject();
                errors.Id = $"Booking with id {bookingId} does not exist.";
                throw new APIException(HttpStatusCode.BadRequest, errors.Id, errors);
            }
            SortedList<string, string> _requestData = AddBookingRequestData(booking, context);
            var data = new StringBuilder();


            foreach (var (key, value) in _requestData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }


            var querystring = data.ToString();


            baseUrl += "?" + querystring;
            var signData = querystring;
            if (signData.Length > 0)
            {
                signData = signData.Remove(data.Length - 1, 1);
            }


            var vnpSecureHash = HmacSha512(hashSecret, signData);
            baseUrl += "vnp_SecureHash=" + vnpSecureHash;


            return baseUrl;
        }

        public async Task<VNPayBookingResponse> CallBackBookingPayment(IQueryCollection collections)
        {
            SortedList<string, string> _responseData = new SortedList<string, string>(new VnPayCompare());
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    _responseData = AddResponseData(key, value, _responseData);
                }
            }
            string code = GetResponseData("vnp_TxnRef", _responseData).Trim();
            float totalAmount = float.Parse(GetResponseData("vnp_Amount", _responseData).Trim()) / 100;
            string name = GetResponseData("vnp_OrderInfo", _responseData).Trim();
            string bankCode = GetResponseData("vnp_BankCode", _responseData).Trim();
            string bankTranNo = GetResponseData("vnp_BankTranNo", _responseData).Trim();
            string cardType = GetResponseData("vnp_CardType", _responseData).Trim();
            DateTime paymentDate = DateTime.ParseExact(GetResponseData("vnp_PayDate", _responseData).Trim(), "yyyyMMddHHmmss", null);
            string VNPayCode = GetResponseData("vnp_TransactionNo", _responseData).Trim();
            string responseCode = GetResponseData("vnp_ResponseCode", _responseData).Trim();
            string transactionStatus = GetResponseData("vnp_TransactionStatus", _responseData).Trim();
            string vnpSecureHash =
                collections.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value;

            string hashSecret = _configuration["VnPay:HashSecret"];
            var checkSignature =
                ValidateSignature(vnpSecureHash, hashSecret, _responseData); //check Signature
            bool success = true;
            if (!checkSignature)
            {
                dynamic errors = new ExpandoObject();
                errors.Id = $"Invalid Signature";
                throw new APIException(HttpStatusCode.BadRequest, errors.Id, errors);
            }
            else
            {
                if (responseCode != "00" || transactionStatus != "00")
                {
                    dynamic errors = new ExpandoObject();
                    errors.Id = "Payment Error";
                    throw new APIException(HttpStatusCode.BadRequest, errors.Id, errors);
                }
            }

            return new VNPayBookingResponse
            {
                Code = code,
                TotalAmount = totalAmount,
                BankCode = bankCode,
                BankTranNo = bankTranNo,
                CardType = cardType,
                Name = name,
                PaymentDate = paymentDate,
                Success = success,
                VNPayCode = VNPayCode
            };
        }

        private string HmacSha512(string key, string inputData)
        {
            var hash = new StringBuilder();
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                var hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }


            return hash.ToString();
        }
        private static string GetIpAddress()
        {

            var ipAddress = string.Empty;
            try
            {
                string hostName = Dns.GetHostName();
                IPAddress[] ipAddresses = Dns.GetHostAddresses(hostName);
                ipAddress = ipAddresses[0].ToString();
                return ipAddress;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            // return "127.0.0.1";
        }
        private SortedList<string, string> AddRequestData(string key, string value, SortedList<string, string> _requestData)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
            return _requestData;
        }

        private SortedList<string, string> AddResponseData(string key, string value, SortedList<string, string> _responseData)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
            return _responseData;
        }

        private string GetResponseData(string key, SortedList<string, string> _responseData)
        {
            return _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;
        }

        private bool ValidateSignature(string inputHash, string secretKey, SortedList<string, string> _responseData)
        {
            var rspRaw = GetResponseData(_responseData);
            var myChecksum = HmacSha512(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private string GetResponseData(SortedList<string, string> _responseData)
        {
            var data = new StringBuilder();
            if (_responseData.ContainsKey("vnp_SecureHashType"))
            {
                _responseData.Remove("vnp_SecureHashType");
            }


            if (_responseData.ContainsKey("vnp_SecureHash"))
            {
                _responseData.Remove("vnp_SecureHash");
            }


            foreach (var (key, value) in _responseData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }


            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }


            return data.ToString();
        }

    }
    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}