using Online_Learning_Platform_Ass1.Service.DTOs.Payment;

namespace Online_Learning_Platform_Ass1.Service.Services.Interfaces;

public interface IVnPayService
{
    string CreatePaymentUrl(string ipAddress, VnPayRequestModel model);
    PaymentResponseModel PaymentExecute(IDictionary<string, string> queryParameters);
}

public class PaymentResponseModel
{
    public bool Success { get; set; }
    public string OrderDescription { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public string VnPayResponseCode { get; set; } = string.Empty;
    
    // Additional fields if needed
    public decimal Amount { get; set; }
}
