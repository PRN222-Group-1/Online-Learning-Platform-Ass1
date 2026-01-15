namespace Online_Learning_Platform_Ass1.Service.DTOs.Payment;

public class VnPayRequestModel
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}
