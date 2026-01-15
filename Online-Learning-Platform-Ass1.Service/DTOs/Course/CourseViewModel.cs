namespace Online_Learning_Platform_Ass1.Service.DTOs.Course;

public record CourseViewModel
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public string? ImageUrl { get; init; }
    public string InstructorName { get; init; } = null!;
    public string CategoryName { get; init; } = null!;
}

public record CourseDetailViewModel : CourseViewModel
{
    public IEnumerable<ModuleViewModel> Modules { get; init; } = new List<ModuleViewModel>();
    public bool IsEnrolled { get; init; }
}

public record ModuleViewModel
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public IEnumerable<LessonViewModel> Lessons { get; init; } = new List<LessonViewModel>();
}

public record LessonViewModel
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public string? VideoUrl { get; init; }
}
