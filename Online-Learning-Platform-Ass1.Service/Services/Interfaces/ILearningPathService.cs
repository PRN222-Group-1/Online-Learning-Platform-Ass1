using Online_Learning_Platform_Ass1.Service.DTOs.LearningPath;

namespace Online_Learning_Platform_Ass1.Service.Services.Interfaces;

public interface ILearningPathService
{
    Task<LearningPathViewModel?> GetLearningPathDetailsAsync(Guid id);
    Task<IEnumerable<LearningPathViewModel>> GetFeaturedLearningPathsAsync();
}
