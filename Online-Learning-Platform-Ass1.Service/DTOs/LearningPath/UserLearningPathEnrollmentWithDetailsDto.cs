using Online_Learning_Platform_Ass1.Service.DTOs.Course;

namespace Online_Learning_Platform_Ass1.Service.DTOs.LearningPath;

public record UserLearningPathEnrollmentWithDetailsDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid PathId { get; init; }
    public DateTime EnrolledAt { get; init; }
    public DateTime? CompletedAt { get; init; }
    public string Status { get; init; } = "active";
    public int ProgressPercentage { get; init; }

    // Path details
    public string PathTitle { get; init; } = null!;
    public string? PathDescription { get; init; }
    public decimal PathPrice { get; init; }
    public int TotalCourses { get; init; }
    public int CompletedCourses { get; init; }

    // List of courses in the path
    public IEnumerable<CourseViewModel> Courses { get; init; } = new List<CourseViewModel>();
}
