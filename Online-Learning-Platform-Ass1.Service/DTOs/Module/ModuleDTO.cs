namespace Online_Learning_Platform_Ass1.Service.DTOs.Module;

public class ModuleDTO
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    public DateTime CreatedAt { get; set; }
}
