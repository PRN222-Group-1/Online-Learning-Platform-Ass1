using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.DTOs.Course;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;
public class CourseService (ICourseRepository courseRepository) : ICourseService
{
    private readonly ICourseRepository _courseRepository = courseRepository;
    public async Task<IEnumerable<CourseDTO>> GetAllAsync()
    {
        var courses = await _courseRepository.GetAllAsync();

        return courses.Select(c => new CourseDTO
        {
            Id = c.Id,
            Title = c.Title,
            Author = c.Author,
            Description = c.Description,
            PictureUrl = c.PictureUrl,
            CreatedAt = c.CreatedAt
        });
    }

    public async Task<CourseDTO?> GetByIdAsync(int courseId)
    {
        var c = await _courseRepository.GetByIdAsync(courseId);
        if (c == null) return null;

        return new CourseDTO
        {
            Id = c.Id,
            Title = c.Title,
            Author = c.Author,
            Description = c.Description,
            PictureUrl = c.PictureUrl,
            CreatedAt = c.CreatedAt
        };
    }

}
