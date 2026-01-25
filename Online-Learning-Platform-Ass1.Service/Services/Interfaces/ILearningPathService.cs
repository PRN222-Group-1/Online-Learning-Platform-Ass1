using Online_Learning_Platform_Ass1.Service.DTOs.LearningPath;

namespace Online_Learning_Platform_Ass1.Service.Services.Interfaces;

public interface ILearningPathService
{
    /// <summary>
    /// Get learning path details with courses
    /// </summary>
    Task<LearningPathViewModel?> GetLearningPathDetailsAsync(Guid id);

    /// <summary>
    /// Get featured learning paths (by popularity)
    /// </summary>
    Task<IEnumerable<LearningPathViewModel>> GetFeaturedLearningPathsAsync();

    /// <summary>
    /// Get all published learning paths
    /// </summary>
    Task<IEnumerable<LearningPathViewModel>> GetPublishedPathsAsync();

    /// <summary>
    /// Get all learning paths a user is enrolled in with progress
    /// </summary>
    Task<IEnumerable<UserLearningPathWithProgressDto>> GetUserEnrolledPathsAsync(Guid userId);

    /// <summary>
    /// Get detailed view of user's enrollment in a specific path
    /// </summary>
    Task<UserLearningPathWithProgressDto?> GetUserPathProgressAsync(Guid userId, Guid pathId);

    /// <summary>
    /// Get learning path details with user's progress (if enrolled)
    /// </summary>
    Task<LearningPathDetailsWithProgressDto?> GetPathDetailsWithProgressAsync(Guid pathId, Guid? userId = null);
}
