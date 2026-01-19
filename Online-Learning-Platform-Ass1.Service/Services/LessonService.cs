using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;
public class LessonService(ILessonRepository lessonRepository) : ILessonService
{
    private readonly ILessonRepository _lessonRepository = lessonRepository;
    public async Task<IEnumerable<Lesson>> GetAllAsync()
    {
        return await _lessonRepository.GetAllAsync();
    }
    public async Task<Lesson?> GetByIdAsync(int lessonId)
    {
        return await _lessonRepository.GetByIdAsync(lessonId);
    }

    public async Task<IEnumerable<Lesson>> GetByModuleIdAsync(int moduleId)
    {
        return await _lessonRepository.GetByModuleIdAsync(moduleId);
    }

    public async Task AddAsync(Lesson lesson)
    {
        await _lessonRepository.AddAsync(lesson);
    }

    public async Task UpdateAsync(Lesson lesson)
    {
        await _lessonRepository.UpdateAsync(lesson);
    }

    public async Task DeleteAsync(int lessonId)
    {
        await _lessonRepository.DeleteAsync(lessonId);
    }
}
