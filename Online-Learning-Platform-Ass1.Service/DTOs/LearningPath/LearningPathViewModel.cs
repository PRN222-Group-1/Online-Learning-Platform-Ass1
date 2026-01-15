using Online_Learning_Platform_Ass1.Service.DTOs.Course;

namespace Online_Learning_Platform_Ass1.Service.DTOs.LearningPath;

public record LearningPathViewModel
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public string Status { get; init; } = null!;
    public IEnumerable<CourseViewModel> Courses { get; init; } = new List<CourseViewModel>();
}
