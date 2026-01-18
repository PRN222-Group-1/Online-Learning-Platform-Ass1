namespace Online_Learning_Platform_Ass1.Data.Models;

public class LessonViewModel
{
    public int LessonId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public string VideoUrl { get; set; } = string.Empty;

    public int Duration { get; set; }
    public int OrderIndex { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsCurrent { get; set; }
}
