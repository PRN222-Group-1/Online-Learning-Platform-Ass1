using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;

/// <summary>
/// Service để cập nhật Learning Path progress khi course hoàn thành
/// </summary>
public class LearningPathProgressSyncService(
    ILearningPathEnrollmentService pathEnrollmentService,
    IEnrollmentRepository enrollmentRepository,
    IUserLearningPathEnrollmentRepository userPathEnrollmentRepository) : ILearningPathProgressSyncService
{
    /// <summary>
    /// Cập nhật progress của tất cả learning paths khi course hoàn thành
    /// </summary>
    public async Task SyncProgressAsync(Guid userId, Guid courseId)
    {
        // Lấy tất cả learning path enrollments của user
        var pathEnrollments = await userPathEnrollmentRepository.GetUserEnrollmentsAsync(userId);

        foreach (var pathEnrollment in pathEnrollments)
        {
            // Kiểm tra nếu course này thuộc về path này
            var path = pathEnrollment.LearningPath;
            if (path?.PathCourses.Any(pc => pc.CourseId == courseId) == true)
            {
                // Tính toán tiến độ mới
                var progress = await pathEnrollmentService.CalculateProgressAsync(userId, pathEnrollment.PathId);

                // Nếu path hoàn thành (100%), mark nó là completed
                if (progress >= 100)
                {
                    await pathEnrollmentService.CompletePathAsync(pathEnrollment.Id);
                }
            }
        }
    }

    /// <summary>
    /// Cập nhật enrollment status khi tất cả courses đã hoàn thành
    /// </summary>
    public async Task UpdateEnrollmentStatusAsync(Guid pathEnrollmentId, Guid userId, Guid pathId)
    {
        var courseEnrollments = await enrollmentRepository.GetStudentEnrollmentsAsync(userId);
        var pathEnrollment = await userPathEnrollmentRepository.GetByIdAsync(pathEnrollmentId);

        if (pathEnrollment == null) return;

        var pathCourseIds = pathEnrollment.LearningPath.PathCourses.Select(pc => pc.CourseId).ToHashSet();
        var completedCount = courseEnrollments.Count(ce => pathCourseIds.Contains(ce.CourseId) && ce.Status == "completed");
        var totalCourses = pathCourseIds.Count;

        // Cập nhật status dựa trên progress
        if (completedCount > 0 && completedCount < totalCourses && pathEnrollment.Status == "active")
        {
            await pathEnrollmentService.UpdateEnrollmentStatusAsync(pathEnrollmentId, "in_progress");
        }
        else if (completedCount == totalCourses && pathEnrollment.Status != "completed")
        {
            await pathEnrollmentService.CompletePathAsync(pathEnrollmentId);
        }
    }
}
