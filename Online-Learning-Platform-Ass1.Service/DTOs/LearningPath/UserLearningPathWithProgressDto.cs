using Online_Learning_Platform_Ass1.Service.DTOs.Course;

namespace Online_Learning_Platform_Ass1.Service.DTOs.LearningPath;

/// <summary>
/// Learning path with user's progress information
/// </summary>
public record UserLearningPathWithProgressDto
{
    public Guid Id { get; init; }
    public Guid EnrollmentId { get; init; }
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public string Status { get; init; } = "draft";
    public bool IsCustomPath { get; init; }
    public int TotalCourses { get; init; }
    public int CompletedCourses { get; init; }
    public int ProgressPercentage { get; init; }
    public string EnrollmentStatus { get; init; } = "active";
    public DateTime EnrolledAt { get; init; }
    public DateTime? CompletedAt { get; init; }
    public IEnumerable<CourseViewModel> Courses { get; init; } = new List<CourseViewModel>();
}
