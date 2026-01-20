namespace Online_Learning_Platform_Ass1.Service.DTOs.Lesson;

public class ProgressDTO
{
    public int Id { get; set; }
    public int EnrollmentId { get; set; }
    public int LessonId { get; set; }

    public int WatchedPosition { get; set; }
    public bool IsCompleted { get; set; }

    public DateTime UpdatedAt { get; set; }
}
