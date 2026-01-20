using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;

namespace Online_Learning_Platform_Ass1.Data.Repositories;
public class ProgressRepository : IProgressRepository
{
    private readonly List<LessonProgress> _progresses = new();
    private int _currentId = 1;

    public Task<LessonProgress?> GetByIdAsync(int progressId)
    {
        var result = _progresses.FirstOrDefault(p => p.Id == progressId);

        return Task.FromResult(result);
    }

    public Task<LessonProgress?> GetByEnrollmentAndLessonAsync(int enrollmentId, int lessonId)
    {
        var result = _progresses.FirstOrDefault(p => p.EnrollmentId == enrollmentId && p.LessonId == lessonId);

        return Task.FromResult(result);
    }

    public Task<IEnumerable<LessonProgress>> GetByEnrollmentIdAsync(int enrollmentId)
    {
        var result = _progresses
            .Where(p => p.EnrollmentId == enrollmentId)
            .AsEnumerable();

        return Task.FromResult(result);
    }

    public Task AddAsync(LessonProgress progress)
    {
        progress.Id = _currentId++;
        _progresses.Add(progress);

        return Task.CompletedTask;
    }

    public Task UpdateAsync(LessonProgress progress)
    {
        var existing = _progresses
            .FirstOrDefault(p => p.Id == progress.Id);

        if (existing == null) return Task.CompletedTask;

        existing.EnrollmentId = progress.EnrollmentId;
        existing.LessonId = progress.LessonId;
        existing.IsCompleted = progress.IsCompleted;
        existing.WatchedPosition = progress.WatchedPosition;

        return Task.CompletedTask;
    }

    public Task MarkCompletedAsync(int enrollmentId, int lessonId)
    {
        var progress = _progresses.FirstOrDefault(p =>
            p.EnrollmentId == enrollmentId &&
            p.LessonId == lessonId);

        if (progress != null)
        {
            progress.IsCompleted = true;
        }

        return Task.CompletedTask;
    }

    public Task UpdateWatchedPositionAsync(int enrollmentId, int lessonId, int lastWatchedPosition)
    {
        var progress = _progresses.FirstOrDefault(p =>
            p.EnrollmentId == enrollmentId &&
            p.LessonId == lessonId);

        if (progress == null)
        {
            progress = new LessonProgress
            {
                Id = _currentId++,
                EnrollmentId = enrollmentId,
                LessonId = lessonId,
                WatchedPosition = lastWatchedPosition,
                IsCompleted = false
            };

            _progresses.Add(progress);
        }
        else
        {
            progress.WatchedPosition = lastWatchedPosition;
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int progressId)
    {
        var progress = _progresses
            .FirstOrDefault(p => p.Id == progressId);

        if (progress != null)
        {
            _progresses.Remove(progress);
        }

        return Task.CompletedTask;
    }
}
