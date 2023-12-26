using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
        private SortedList<string, string> AddRegisterRequestData(MembershipPackage membershipPackage, HttpContext context)
        {
            SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
            var tick = DateTime.Now.Ticks.ToString();
            var callBackUrl = "https://" + context.Request.Host + _configuration["PaymentCallBack:MembershipPaymentReturnUrl"];
            //add basic parameter to VNPay 
            _requestData = AddRequestData("vnp_Version", _configuration["Vnpay:Version"], _requestData);
            _requestData = AddRequestData("vnp_Command", _configuration["Vnpay:Command"], _requestData);
            _requestData = AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"], _requestData);
            _requestData = AddRequestData("vnp_Amount", ((int)membershipPackage.Price * 100).ToString(), _requestData);
            _requestData = AddRequestData("vnp_CreateDate", DateTime.UtcNow.AddHours(7).ToString("yyyyMMddHHmmss"), _requestData);
            _requestData = AddRequestData("vnp_CurrCode", "VND", _requestData);
            _requestData = AddRequestData("vnp_IpAddr", GetIpAddress(), _requestData);
            _requestData = AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"], _requestData);
            _requestData = AddRequestData("vnp_OrderInfo", membershipPackage.Id.ToString(), _requestData);
            _requestData = AddRequestData("vnp_OrderType", nameof(EnumTransactionType.Register), _requestData);
            _requestData = AddRequestData("vnp_ReturnUrl", callBackUrl, _requestData);
            _requestData = AddRequestData("vnp_TxnRef", tick, _requestData);
            _requestData = AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss"), _requestData);
            return _requestData;
        }

        public async Task<string?> CreateRegisterRequestURL(Guid membershipPackageId, HttpContext context)
        {
            string baseUrl = _configuration["VnPay:BaseUrl"];
            string hashSecret = _configuration["VnPay:HashSecret"];
            MembershipPackage activedMembershipPackage = await _unitOfWork.MembershipPackageRepository.GetByID(membershipPackageId);
            if (activedMembershipPackage == null)
            {
                string message = "This membership package is currently inactive, please choose another membership package.";
                throw new APIException(HttpStatusCode.BadRequest, message);
            }
            SortedList<string, string> _requestData = AddRegisterRequestData(activedMembershipPackage, context);
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

        public async Task<VNPayResponseModel> CallBackRegisterPayment(IQueryCollection collections)
        {
            SortedList<string, string> _responseData = new SortedList<string, string>(new VnPayCompare());
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    _responseData = AddResponseData(key, value, _responseData);
                }
            }
            Guid membershipPackageId = Guid.Parse(GetResponseData("vnp_OrderInfo", _responseData).Trim()); //membership package id //can lay
            string responseCode = GetResponseData("vnp_ResponseCode", _responseData).Trim();// can lay
            DateTime paymentDate = DateTime.ParseExact(GetResponseData("vnp_PayDate", _responseData).Trim(), "yyyyMMddHHmmss", null);
            string transactionNo = GetResponseData("vnp_TransactionNo", _responseData).Trim();//can lay
            string transactionStatus = GetResponseData("vnp_TransactionStatus", _responseData).Trim();//can lay
            string vnpSecureHash =
                collections.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value; //hash của dữ liệu trả về

            string hashSecret = _configuration["VnPay:HashSecret"];
            var checkSignature =
                ValidateSignature(vnpSecureHash, hashSecret, _responseData); //check Signature
            if (!checkSignature)
            {
                string message = "Invalid Signature";
                throw new APIException(HttpStatusCode.BadGateway, message);
            }

            return new VNPayResponseModel
            {
                MembershipPackageId = membershipPackageId,
                TransactionNumber = transactionNo,
                TransactionStatus = transactionStatus,
                VnpayPaymentDate = paymentDate,
                VnpayResponseCode = responseCode
            };
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
}