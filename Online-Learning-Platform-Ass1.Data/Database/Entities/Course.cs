namespace Online_Learning_Platform_Ass1.Data.Database.Entities;
public class Course
{
    public int Id { get; set; }
    public string PictureUrl { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<CourseModule> Modules { get; set; } = new List<CourseModule>();
}
