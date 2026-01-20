namespace Online_Learning_Platform_Ass1.Service.DTOs.Course;

public class CourseDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string PictureUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
