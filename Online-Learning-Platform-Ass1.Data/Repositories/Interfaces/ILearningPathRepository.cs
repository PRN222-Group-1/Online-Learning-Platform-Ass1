using Online_Learning_Platform_Ass1.Data.Database.Entities;

namespace Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;

public interface ILearningPathRepository
{
    Task<LearningPath?> GetByIdAsync(Guid id);
    Task<IEnumerable<LearningPath>> GetAllAsync();
    Task<IEnumerable<LearningPath>> GetPublishedPathsAsync();
    Task<IEnumerable<LearningPath>> GetFeaturedPathsAsync(int count = 5);
    Task<IEnumerable<LearningPath>> GetUserCustomPathsAsync(Guid userId);
    Task<LearningPath?> GetByAssessmentIdAsync(Guid assessmentId);
    Task AddAsync(LearningPath path);
    Task AddPathCourseAsync(PathCourse pathCourse);
    Task UpdateAsync(LearningPath path);
    Task DeleteAsync(Guid id);
    Task SaveChangesAsync();
}
