namespace Online_Learning_Platform_Ass1.Data.Models;

public class CourseViewModel
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // bên trái: bài đang xem
    public int? CurrentLessonId { get; set; }
    public LessonViewModel CurrentLesson { get; set; } = null!;

    //bên phải
    public List<ModuleViewModel> Modules { get; set; } = new();
}
