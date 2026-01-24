using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Online_Learning_Platform_Ass1.Service.DTOs.Payment;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;

public class VnPayService : IVnPayService
{
    private readonly IConfiguration _configuration;

    public VnPayService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreatePaymentUrl(string ipAddress, VnPayRequestModel model)
    {
        string vnp_TmnCode = _configuration["VnPay:TmnCode"] ?? string.Empty;
        string vnp_HashSecret = _configuration["VnPay:HashSecret"] ?? string.Empty;
        string vnp_Url = _configuration["VnPay:BaseUrl"] ?? string.Empty;
        string vnp_ReturnUrl = _configuration["VnPay:CallbackUrl"] ?? string.Empty;

        // Adjust IP for localhost
        if (ipAddress == "::1" || ipAddress == "0.0.0.1")
        {
            ipAddress = "127.0.0.1";
        }

        var vnpay = new VnPayLibrary();
        vnpay.AddRequestData("vnp_Version", "2.1.0");
        vnpay.AddRequestData("vnp_Command", "pay");
        vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
        vnpay.AddRequestData("vnp_Amount", ((long)(model.Amount * 100)).ToString());
        vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
        vnpay.AddRequestData("vnp_CurrCode", "VND");
        vnpay.AddRequestData("vnp_IpAddr", ipAddress);
        vnpay.AddRequestData("vnp_Locale", "vn");
        vnpay.AddRequestData("vnp_OrderInfo", "ThanhToanDonHang");
        vnpay.AddRequestData("vnp_OrderType", "other");
        vnpay.AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
        vnpay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); 

        string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
        
        // Log for debugging
        Console.WriteLine($"VNPAY REQUEST URL: {paymentUrl}");

        return paymentUrl;
    }

    public PaymentResponseModel PaymentExecute(IDictionary<string, string> queryParameters)
    {
        string vnp_HashSecret = _configuration["VnPay:HashSecret"] ?? string.Empty;

        var vnpay = new VnPayLibrary();
        foreach (var param in queryParameters)
        {
            if (!string.IsNullOrEmpty(param.Key) && param.Key.StartsWith("vnp_"))
            {
                vnpay.AddResponseData(param.Key, param.Value);
            }
        }

        string vnp_SecureHash = "";
        if (queryParameters.ContainsKey("vnp_SecureHash"))
        {
            vnp_SecureHash = queryParameters["vnp_SecureHash"];
        }

        Console.WriteLine($"VNPAY RESPONSE HASH: {vnp_SecureHash}");
        
        bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

        if (!checkSignature)
        {
             Console.WriteLine("VNPAY SIGNATURE CHECK FAILED!");
             return new PaymentResponseModel
            {
                Success = false,
                VnPayResponseCode = "InvalidSignature"
            };
        }

        return new PaymentResponseModel
        {
            Success = true,
            PaymentMethod = "VnPay",
            OrderDescription = vnpay.GetResponseData("vnp_OrderInfo"),
            OrderId = vnpay.GetResponseData("vnp_TxnRef"),
            TransactionId = vnpay.GetResponseData("vnp_TransactionNo"),
            VnPayResponseCode = vnpay.GetResponseData("vnp_ResponseCode")
        };
    }
}

public class VnPayLibrary
{
    private readonly SortedList<string, string> _requestData = new SortedList<string, string>(StringComparer.Ordinal);
    private readonly SortedList<string, string> _responseData = new SortedList<string, string>(StringComparer.Ordinal);

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
        return _responseData.TryGetValue(key, out var result) ? result : string.Empty;
    }

    public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
    {
        StringBuilder data = new StringBuilder();
        foreach (KeyValuePair<string, string> kv in _requestData)
        {
            if (data.Length > 0)
            {
                data.Append('&');
            }
            data.Append(kv.Key + "=" + Uri.EscapeDataString(kv.Value));
        }
        string queryString = data.ToString();
        string vnp_SecureHash = Utils.HmacSHA512(vnp_HashSecret, queryString);
        string paymentUrl = baseUrl + "?" + queryString + "&vnp_SecureHash=" + vnp_SecureHash;
        
        Console.WriteLine("------------------------------------------");
        Console.WriteLine($"VNPAY RAW REQUEST STRING (BEFORE HASH): {queryString}");
        Console.WriteLine($"VNPAY SIGNATURE: {vnp_SecureHash}");
        Console.WriteLine("------------------------------------------");

        return paymentUrl;
    }

    public bool ValidateSignature(string inputHash, string secretKey)
    {
        string rspRaw = GetResponseData();
        string myChecksum = Utils.HmacSHA512(secretKey, rspRaw);
        
        Console.WriteLine("------------------------------------------");
        Console.WriteLine($"MY CHECKSUM: {myChecksum}");
        Console.WriteLine($"INPUT HASH: {inputHash}");
        Console.WriteLine($"RAW DATA TO HASH: {rspRaw}");
        Console.WriteLine("------------------------------------------");

        return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
    }

    private string GetResponseData()
    {
        StringBuilder data = new StringBuilder();
        if (_responseData.ContainsKey("vnp_SecureHash"))
        {
            _responseData.Remove("vnp_SecureHash");
        }
        if (_responseData.ContainsKey("vnp_SecureHashType"))
        {
            _responseData.Remove("vnp_SecureHashType");
        }
        
        foreach (KeyValuePair<string, string> kv in _responseData)
        {
             if (data.Length > 0)
            {
                data.Append('&');
            }
            data.Append(kv.Key + "=" + Uri.EscapeDataString(kv.Value));
        }
        return data.ToString();
    }
}

public static class Utils
{
    public static string HmacSHA512(string key, string inputData)
    {
        var hash = new StringBuilder();
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
        using (var hmac = new HMACSHA512(keyBytes))
        {
            byte[] hashValue = hmac.ComputeHash(inputBytes);
            foreach (var theByte in hashValue)
            {
                hash.Append(theByte.ToString("x2"));
            }
        }
        return hash.ToString();
    }
}
