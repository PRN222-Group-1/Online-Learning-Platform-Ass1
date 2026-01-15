namespace Online_Learning_Platform_Ass1.Service.DTOs.Order;

public record CreateOrderDto
{
    public Guid? CourseId { get; init; }
    public Guid? PathId { get; init; }
}

public record OrderViewModel
{
    public Guid OrderId { get; init; }
    public Guid CourseId { get; init; }
    public string CourseTitle { get; init; } = null!;
    public decimal Amount { get; init; }
    public string Status { get; init; } = null!;
}
