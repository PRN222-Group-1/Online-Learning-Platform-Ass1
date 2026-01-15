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
}
