
public interface ILessonService
{
    Task<IEnumerable<LessonDTO>> GetAllAsync();
    Task<LessonDTO?> GetByIdAsync(int lessonId);
    Task<IEnumerable<LessonDTO>> GetByModuleIdAsync(int moduleId);

    Task UpdateAsync(LessonDTO lesson);


}
