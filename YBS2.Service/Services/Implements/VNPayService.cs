using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using YBS2.Data.Enums;
using YBS2.Data.Models;
using YBS2.Service.Dtos.Inputs;

namespace YBS2.Service.Services.Implements
{
    public class VNPayService : IVNPayService
    {
        private readonly IConfiguration _configuration;
        public VNPayService(IConfiguration configuration)
        {
            _configuration = configuration;
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
        private SortedList<string, string> AddRegisterRequestData(MemberInputDto inputDto, MembershipPackage membershipPackage, HttpContext context)
        {
            SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
            var tick = DateTime.Now.Ticks.ToString();
            var callBackUrl = "https://" + context.Request.Host + _configuration["PaymentCallBack:BookingPaymentReturnUrl"];
            //add basic parameter to VNPay 
            _requestData = AddRequestData("vnp_Version", _configuration["Vnpay:Version"], _requestData);
            _requestData = AddRequestData("vnp_Command", _configuration["Vnpay:Command"], _requestData);
            _requestData = AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"], _requestData);
            _requestData = AddRequestData("vnp_Amount", ((int)membershipPackage.Price * 100).ToString(), _requestData);
            _requestData = AddRequestData("vnp_CreateDate", DateTime.UtcNow.AddHours(7).ToString("yyyyMMddHHmmss"), _requestData);
            _requestData = AddRequestData("vnp_CurrCode", "VND", _requestData);
            _requestData = AddRequestData("vnp_IpAddr", GetIpAddress(), _requestData);
            _requestData = AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"], _requestData);
            _requestData = AddRequestData("vnp_OrderInfo", membershipPackage.Name, _requestData);
            _requestData = AddRequestData("vnp_OrderType", nameof(EnumTransactionType.Register), _requestData);
            _requestData = AddRequestData("vnp_ReturnUrl", callBackUrl, _requestData);
            _requestData = AddRequestData("vnp_TxnRef", tick, _requestData);
            _requestData = AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss"), _requestData);
            return _requestData;
        }

        public async Task<string> CreateRegisterRequestURL(MemberInputDto inputDto, MembershipPackage membershipPackage, HttpContext context, string baseUrl, string vnpHashSecret)
        {
            SortedList<string, string> _requestData = AddRegisterRequestData(inputDto, membershipPackage, context);
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


            var vnpSecureHash = HmacSha512(vnpHashSecret, signData);
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
    }
}