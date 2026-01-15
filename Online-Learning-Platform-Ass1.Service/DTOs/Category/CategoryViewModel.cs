namespace Online_Learning_Platform_Ass1.Service.DTOs.Category;

public record CategoryViewModel
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
}
