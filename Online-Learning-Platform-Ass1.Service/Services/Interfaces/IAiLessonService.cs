using Online_Learning_Platform_Ass1.Service.DTOs.Lesson;

public interface IAiLessonService
{
    Task<string> GenerateSummaryAsync(LessonDTO lesson);
    Task<string> AskAsync(LessonDTO lesson, string question);
}
