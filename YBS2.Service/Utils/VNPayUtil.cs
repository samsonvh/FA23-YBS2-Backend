using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
public class VNPayUtil
{
    private readonly SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
    private readonly SortedList<string, string> _responseData = new SortedList<string, string>(new VnPayCompare());

    // public MembershipPaymentResponseModel GetFullMembershipPaymentResponseData(IQueryCollection collection, string hashSecret, IMemoryCache memoryCache, string cachedKey)
    // {
    //     var vnPay = new VNPayUtil();


    //     foreach (var (key, value) in collection)
    //     {
    //         if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
    //         {
    //             vnPay.AddResponseData(key, value);
    //         }
    //     }
    //     var tmnCode = vnPay.GetResponseData("vnp_TmnCode").Trim();
    //     var txnRef = vnPay.GetResponseData("vnp_TxnRef").Trim();
    //     var amount = float.Parse(vnPay.GetResponseData("vnp_Amount")) / 100;
    //     var transactionName = vnPay.GetResponseData("vnp_OrderInfo").Trim(); //transaction name
    //     var responseCode = vnPay.GetResponseData("vnp_ResponseCode").Trim();
    //     var bankCode = vnPay.GetResponseData("vnp_BankCode").Trim();
    //     var cardType = vnPay.GetResponseData("vnp_CardType").Trim();
    //     var payDate = vnPay.GetResponseData("vnp_PayDate").Trim();
    //     var transactionNo = vnPay.GetResponseData("vnp_TransactionNo").Trim();
    //     var transactionStatus = vnPay.GetResponseData("vnp_TransactionStatus").Trim();
    //     var vnpSecureHash =
    //         collection.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value; //hash của dữ liệu trả về


    //     var checkSignature =
    //         vnPay.ValidateSignature(vnpSecureHash, hashSecret); //check Signature


    //     if (!checkSignature)
    //         return new MembershipPaymentResponseModel()
    //         {
    //             Success = false
    //         };
    //     var result = new MembershipPaymentResponseModel()
    //     {
    //         Success = true,
    //         //Transaction field
    //         TransactionName = transactionName,
    //         Amount = amount,
    //         //field response from vnpay not exist in db
    //         TmnCode = tmnCode,
    //         TxnRef = txnRef,
    //         ResponseCode = responseCode,
    //         BankCode = bankCode,
    //         cardType = cardType,
    //         TransactionNo = transactionNo,
    //         TransactionStatus = transactionStatus,
    //         SecureHash = vnpSecureHash,
    //     };
    //     //retrieve object from cache
    //     MembershipPackageInformationInputDto membershipPackageInformationInputDto;
    //     if (memoryCache.TryGetValue(cachedKey, out membershipPackageInformationInputDto))
    //     {
    //         result.MembershipPackageId = membershipPackageInformationInputDto.MembershipPackageId;
    //         result.TransactionType = membershipPackageInformationInputDto.TransactionType.ToString();
    //         result.PaymentMethod = membershipPackageInformationInputDto.PaymentMethod.ToString();
    //         result.MoneyUnit = membershipPackageInformationInputDto.MoneyUnit;
    //         result.Username = membershipPackageInformationInputDto.Username;
    //         result.Password = membershipPackageInformationInputDto.Password;
    //         result.DateOfBirth = membershipPackageInformationInputDto.DateOfBirth;
    //         result.Nationality = membershipPackageInformationInputDto.Nationality;
    //         result.Gender = membershipPackageInformationInputDto.Gender; 
    //         result.Address = membershipPackageInformationInputDto.Address;
    //         result.IdentityNumber = membershipPackageInformationInputDto.IdentityNumber;
    //         result.Email = membershipPackageInformationInputDto.Email;
    //         result.FullName = membershipPackageInformationInputDto.FullName;
    //         result.PhoneNumber = membershipPackageInformationInputDto.PhoneNumber;
    //     }
    //     memoryCache.Remove(cachedKey);
    //     memoryCache.Dispose();

    //     if (DateTime.TryParseExact(payDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
    //     {
    //         result.PaymentDate = date;
    //     }
    //     return result;
    // }
    public static string GetIpAddress()
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
    public void AddRequestData(string key, string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            _requestData.Add(key, value);
        }
    }


    public void AddResponseData(string key, string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            _responseData.Add(key, value);
        }
    }


    public string GetResponseData(string key)
    {
        return _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;
    }


    public string CreateRequestUrl(string baseUrl, string vnpHashSecret)
    {
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


    public bool ValidateSignature(string inputHash, string secretKey)
    {
        var rspRaw = GetResponseData();
        var myChecksum = HmacSha512(secretKey, rspRaw);
        return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
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


    private string GetResponseData()
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
