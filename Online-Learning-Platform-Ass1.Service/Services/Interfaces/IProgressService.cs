using Online_Learning_Platform_Ass1.Service.DTOs.Lesson;

namespace Online_Learning_Platform_Ass1.Service.Services.Interfaces;

public interface IProgressService
{
    Task<ProgressDTO?> GetLessonProgressAsync(int enrollmentId, int lessonId);
    Task<IEnumerable<ProgressDTO>> GetCourseProgressAsync(int enrollmentId);

    Task UpdateWatchedPositionAsync(int enrollmentId, int lessonId, int watchedPosition);
    Task CompleteLessonAsync(int enrollmentId, int lessonId);

    Task<double> GetCourseCompletionPercentageAsync(int enrollmentId);
}
