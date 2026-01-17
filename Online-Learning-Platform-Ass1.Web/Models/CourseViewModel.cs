using Online_Learning_Platform_Ass1.Service.DTOs.Lesson;
using Online_Learning_Platform_Ass1.Service.DTOs.Module;

public class CourseViewModel
{
    public IEnumerable<ModuleDTO> Modules { get; set; } = null!;
    public IEnumerable<LessonDTO> Lessons { get; set; } = null!;

    }
