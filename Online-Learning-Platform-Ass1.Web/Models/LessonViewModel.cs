
namespace Online_Learning_Platform_Ass1.Web.Models;

public enum AiSummaryStatus
{
    None = 0,
    Processing = 1,
    Done = 2,
    Failed = 3
}

public class LessonViewModel
{
    public int LessonId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public string VideoUrl { get; set; } = string.Empty;

    public string? Transcript { get; set; }
    public string? AiSummary { get; set; }
    public AiSummaryStatus AiSummaryStatus { get; set; } = AiSummaryStatus.None;

    public int Duration { get; set; }
    public int OrderIndex { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsCurrent { get; set; }
}
