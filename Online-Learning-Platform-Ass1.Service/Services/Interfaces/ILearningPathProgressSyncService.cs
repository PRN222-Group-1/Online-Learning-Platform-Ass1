namespace Online_Learning_Platform_Ass1.Service.Services.Interfaces;

/// <summary>
/// Service để sync Learning Path progress khi course hoàn thành
/// </summary>
public interface ILearningPathProgressSyncService
{
    /// <summary>
    /// Cập nhật tiến độ của tất cả learning paths chứa course này
    /// </summary>
    Task SyncProgressAsync(Guid userId, Guid courseId);

    /// <summary>
    /// Cập nhật trạng thái enrollment dựa trên course completions
    /// </summary>
    Task UpdateEnrollmentStatusAsync(Guid pathEnrollmentId, Guid userId, Guid pathId);
}
