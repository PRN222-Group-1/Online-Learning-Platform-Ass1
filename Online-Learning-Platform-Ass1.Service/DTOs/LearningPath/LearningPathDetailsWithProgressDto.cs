using Online_Learning_Platform_Ass1.Service.DTOs.Course;

namespace Online_Learning_Platform_Ass1.Service.DTOs.LearningPath;

/// <summary>
/// Detailed learning path view with user progress
/// </summary>
public record LearningPathDetailsWithProgressDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public string Status { get; init; } = "draft";
    
    // Course information
    public int TotalCourses { get; init; }
    public IEnumerable<CourseViewModel> Courses { get; init; } = new List<CourseViewModel>();

    // User progress (nullable if not enrolled)
    public Guid? EnrollmentId { get; init; }
    public bool IsEnrolled { get; init; }
    public string? EnrollmentStatus { get; init; }
    public int ProgressPercentage { get; init; }
    public int CompletedCourses { get; init; }
    public DateTime? EnrolledAt { get; init; }
    public DateTime? CompletedAt { get; init; }
}
