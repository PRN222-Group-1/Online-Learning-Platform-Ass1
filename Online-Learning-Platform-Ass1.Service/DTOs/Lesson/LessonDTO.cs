using Online_Learning_Platform_Ass1.Data.Database.Entities;

public class LessonDTO
{

    public int Id { get; set; }
    public int ModuleId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public int Duration { get; set; }

    public string? AiSummary { get; set; }
    public string? Transcript { get; set; }
    public AiSummaryStatus AiSummaryStatus { get; set; }

    public int OrderIndex { get; set; }
    public DateTime CreatedAt { get; set; }
}
