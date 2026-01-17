using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.DTOs.Lesson;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;
public class LessonService (ILessonRepository lessonRepository) : ILessonService
{
    private readonly ILessonRepository _lessonRepository = lessonRepository;

    public List<LessonDTO> GetLessonsByModuleId(int moduleId)
    {
        return _lessonRepository.GetLessonsByModuleId(moduleId)
            .Select(lesson => new LessonDTO(lesson.Id, lesson.ModuleId, lesson.Title, lesson.Content, lesson.VideoUrl, lesson.Duration, lesson.OrderIndex, lesson.CreatedAt))
            .ToList();
    }
}
