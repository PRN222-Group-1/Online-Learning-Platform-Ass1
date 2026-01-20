using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;

public class LessonService(ILessonRepository lessonRepository) : ILessonService
{
    private readonly ILessonRepository _lessonRepository = lessonRepository;

    public async Task<IEnumerable<LessonDTO>> GetAllAsync()
    {
        var lessons = await _lessonRepository.GetAllAsync();

        return lessons.Select(l => new LessonDTO
        {
            Id = l.Id,
            ModuleId = l.ModuleId,
            Title = l.Title,
            Content = l.Content,
            VideoUrl = l.VideoUrl,
            Duration = l.Duration,
            AiSummary = l.AiSummary,
            Transcript = l.Transcript,
            AiSummaryStatus = l.AiSummaryStatus,
            OrderIndex = l.OrderIndex,
            CreatedAt = l.CreatedAt
        });
    }

    public async Task<LessonDTO?> GetByIdAsync(int lessonId)
    {
        var l = await _lessonRepository.GetByIdAsync(lessonId);
        if (l == null) return null;

        return new LessonDTO
        {
            Id = l.Id,
            ModuleId = l.ModuleId,
            Title = l.Title,
            Content = l.Content,
            VideoUrl = l.VideoUrl,
            Duration = l.Duration,
            AiSummary = l.AiSummary,
            Transcript = l.Transcript,
            AiSummaryStatus = l.AiSummaryStatus,
            OrderIndex = l.OrderIndex,
            CreatedAt = l.CreatedAt
        };
    }

    public async Task<IEnumerable<LessonDTO>> GetByModuleIdAsync(int moduleId)
    {
        var lessons = await _lessonRepository.GetByModuleIdAsync(moduleId);

        return lessons.Select(l => new LessonDTO
        {
            Id = l.Id,
            ModuleId = l.ModuleId,
            Title = l.Title,
            Content = l.Content,
            VideoUrl = l.VideoUrl,
            Duration = l.Duration,
            AiSummary = l.AiSummary,
            Transcript = l.Transcript,
            AiSummaryStatus = l.AiSummaryStatus,
            OrderIndex = l.OrderIndex,
            CreatedAt = l.CreatedAt
        });
    }

    public async Task UpdateAsync(LessonDTO dto)
    {
        var lesson = await _lessonRepository.GetByIdAsync(dto.Id);
        if (lesson == null)
            throw new Exception("Lesson not found");

        lesson.Title = dto.Title;
        lesson.Content = dto.Content;
        lesson.VideoUrl = dto.VideoUrl;
        lesson.Duration = dto.Duration;
        lesson.AiSummary = dto.AiSummary;
        lesson.Transcript = dto.Transcript;
        lesson.AiSummaryStatus = dto.AiSummaryStatus;
        lesson.OrderIndex = dto.OrderIndex;

        await _lessonRepository.UpdateAsync(lesson);
    }
}
