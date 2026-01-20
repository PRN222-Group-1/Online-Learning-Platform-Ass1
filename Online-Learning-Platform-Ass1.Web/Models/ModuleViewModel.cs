namespace Online_Learning_Platform_Ass1.Data.Models;

public class ModuleViewModel
{
    public int ModuleId { get; set; }
    public string Title { get; set; } = string.Empty;

    public List<LessonViewModel> Lessons { get; set; } = new();

}
