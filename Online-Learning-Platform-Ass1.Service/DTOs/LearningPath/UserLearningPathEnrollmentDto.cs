namespace Online_Learning_Platform_Ass1.Service.DTOs.LearningPath;

public record UserLearningPathEnrollmentDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid PathId { get; init; }
    public DateTime EnrolledAt { get; init; }
    public DateTime? CompletedAt { get; init; }
    public string Status { get; init; } = "active";
    public int ProgressPercentage { get; init; }
}
