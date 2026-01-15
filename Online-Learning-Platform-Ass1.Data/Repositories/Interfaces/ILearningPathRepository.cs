using Online_Learning_Platform_Ass1.Data.Database.Entities;

namespace Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;

public interface ILearningPathRepository
{
    Task<LearningPath?> GetByIdAsync(Guid id);
    Task<IEnumerable<LearningPath>> GetAllAsync();
}
