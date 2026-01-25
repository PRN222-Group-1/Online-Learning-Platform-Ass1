using Online_Learning_Platform_Ass1.Service.DTOs.LearningPath;

namespace Online_Learning_Platform_Ass1.Service.Services.Interfaces;

public interface ILearningPathEnrollmentService
{
    /// <summary>
    /// Enroll a user in a learning path
    /// </summary>
    Task<UserLearningPathEnrollmentDto?> EnrollUserAsync(Guid userId, Guid pathId);

    /// <summary>
    /// Get user's enrollment in a specific learning path
    /// </summary>
    Task<UserLearningPathEnrollmentDto?> GetEnrollmentAsync(Guid enrollmentId);

    /// <summary>
    /// Get all learning paths a user is enrolled in
    /// </summary>
    Task<IEnumerable<UserLearningPathEnrollmentDto>> GetUserEnrollmentsAsync(Guid userId);

    /// <summary>
    /// Check if user is enrolled in a learning path
    /// </summary>
    Task<bool> IsEnrolledAsync(Guid userId, Guid pathId);

    /// <summary>
    /// Update enrollment status
    /// </summary>
    Task<bool> UpdateEnrollmentStatusAsync(Guid enrollmentId, string status);

    /// <summary>
    /// Calculate and update overall progress for a learning path based on course enrollments
    /// </summary>
    Task<int> CalculateProgressAsync(Guid userId, Guid pathId);

    /// <summary>
    /// Mark a learning path as completed
    /// </summary>
    Task<bool> CompletePathAsync(Guid enrollmentId);

    /// <summary>
    /// Drop a learning path enrollment
    /// </summary>
    Task<bool> DropPathAsync(Guid enrollmentId);

    /// <summary>
    /// Get learning path enrollment details with full path information
    /// </summary>
    Task<UserLearningPathEnrollmentWithDetailsDto?> GetEnrollmentDetailsAsync(Guid enrollmentId);
}
