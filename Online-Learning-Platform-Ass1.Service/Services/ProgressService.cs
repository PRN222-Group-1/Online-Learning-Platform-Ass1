using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.DTOs.Lesson;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;

public class ProgressService(IProgressRepository progressRepository, ILessonRepository lessonRepository) : IProgressService
{
    private readonly IProgressRepository _progressRepository = progressRepository;
    private readonly ILessonRepository _lessonRepository = lessonRepository;

    public async Task<ProgressDTO?> GetLessonProgressAsync(int enrollmentId, int lessonId)
    {
        var progress =
            await _progressRepository.GetByEnrollmentAndLessonAsync(enrollmentId, lessonId);

        if (progress == null) return null;

        return new ProgressDTO
        {
            Id = progress.Id,
            EnrollmentId = progress.EnrollmentId,
            LessonId = progress.LessonId,
            WatchedPosition = progress.WatchedPosition,
            IsCompleted = progress.IsCompleted,
            UpdatedAt = progress.UpdatedAt
        };
    }

    public async Task<IEnumerable<ProgressDTO>> GetCourseProgressAsync(int enrollmentId)
    {
        var progresses =
            await _progressRepository.GetByEnrollmentIdAsync(enrollmentId);

        return progresses.Select(p => new ProgressDTO
        {
            Id = p.Id,
            EnrollmentId = p.EnrollmentId,
            LessonId = p.LessonId,
            WatchedPosition = p.WatchedPosition,
            IsCompleted = p.IsCompleted,
            UpdatedAt = p.UpdatedAt
        });
    }

    public async Task UpdateWatchedPositionAsync(
        int enrollmentId,
        int lessonId,
        int watchedPosition
    )
    {
        await _progressRepository.UpdateWatchedPositionAsync(
            enrollmentId,
            lessonId,
            watchedPosition
        );
    }

    public async Task CompleteLessonAsync(int enrollmentId, int lessonId)
    {
        await _progressRepository.MarkCompletedAsync(enrollmentId, lessonId);
    }

    public async Task<double> GetCourseCompletionPercentageAsync(int enrollmentId)
    {
        var progresses =
            (await _progressRepository.GetByEnrollmentIdAsync(enrollmentId))
            .ToList();

        if (!progresses.Any())
            return 0;

        var completedCount = progresses.Count(p => p.IsCompleted);
        var totalCount = progresses.Count;

        return (double)completedCount / totalCount * 100;
    }
}
