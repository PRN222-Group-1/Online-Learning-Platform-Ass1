namespace Online_Learning_Platform_Ass1.Service.DTOs.Blog;

public class BlogCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Status { get; set; } = "draft"; // Default to draft
}
