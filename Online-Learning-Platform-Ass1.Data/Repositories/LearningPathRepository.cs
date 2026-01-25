using Microsoft.EntityFrameworkCore;
using Online_Learning_Platform_Ass1.Data.Database;
using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;

namespace Online_Learning_Platform_Ass1.Data.Repositories;

public class LearningPathRepository(OnlineLearningContext context) : ILearningPathRepository
{
    public async Task<LearningPath?> GetByIdAsync(Guid id)
    {
        return await context.LearningPaths
            .AsNoTracking()
            .Include(p => p.PathCourses)
                .ThenInclude(pc => pc.Course)
                    .ThenInclude(c => c.Instructor)
            .Include(p => p.PathCourses)
                .ThenInclude(pc => pc.Course)
                    .ThenInclude(c => c.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<LearningPath>> GetAllAsync()
    {
        return await context.LearningPaths
            .AsNoTracking()
            .Include(p => p.PathCourses)
                .ThenInclude(pc => pc.Course)
            .ToListAsync();
    }

    public async Task<IEnumerable<LearningPath>> GetPublishedPathsAsync()
    {
        return await context.LearningPaths
            .AsNoTracking()
            .Include(p => p.PathCourses)
                .ThenInclude(pc => pc.Course)
            .Where(p => p.Status == "published" && !p.IsCustomPath)
            .OrderByDescending(p => p.Price)
            .ToListAsync();
    }

    public async Task<IEnumerable<LearningPath>> GetFeaturedPathsAsync(int count = 5)
    {
        return await context.LearningPaths
            .AsNoTracking()
            .Include(p => p.PathCourses)
                .ThenInclude(pc => pc.Course)
                    .ThenInclude(c => c.Category)
            .Where(p => p.Status == "published" && !p.IsCustomPath)
            .OrderByDescending(p => p.UserEnrollments.Count)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<LearningPath>> GetUserCustomPathsAsync(Guid userId)
    {
        return await context.LearningPaths
            .AsNoTracking()
            .Include(p => p.PathCourses)
                .ThenInclude(pc => pc.Course)
                    .ThenInclude(c => c.Instructor)
            .Include(p => p.PathCourses)
                .ThenInclude(pc => pc.Course)
                    .ThenInclude(c => c.Category)
            .Where(p => p.IsCustomPath && p.CreatedByUserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<LearningPath?> GetByAssessmentIdAsync(Guid assessmentId)
    {
        return await context.LearningPaths
            .AsNoTracking()
            .Include(p => p.PathCourses)
                .ThenInclude(pc => pc.Course)
                    .ThenInclude(c => c.Instructor)
            .Include(p => p.PathCourses)
                .ThenInclude(pc => pc.Course)
                    .ThenInclude(c => c.Category)
            .FirstOrDefaultAsync(p => p.SourceAssessmentId == assessmentId);
    }

    public async Task AddAsync(LearningPath path)
    {
        if (path.Id == Guid.Empty)
            path.Id = Guid.NewGuid();
        await context.LearningPaths.AddAsync(path);
    }

    public async Task AddPathCourseAsync(PathCourse pathCourse)
    {
        await context.Set<PathCourse>().AddAsync(pathCourse);
    }

    public async Task UpdateAsync(LearningPath path)
    {
        context.LearningPaths.Update(path);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var path = await context.LearningPaths.FindAsync(id);
        if (path != null)
        {
            context.LearningPaths.Remove(path);
            await context.SaveChangesAsync();
        }
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
