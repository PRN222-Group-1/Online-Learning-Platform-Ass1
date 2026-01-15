using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.DTOs.Course;
using Online_Learning_Platform_Ass1.Service.DTOs.LearningPath;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;

public class LearningPathService(ILearningPathRepository learningPathRepository) : ILearningPathService
{
    public async Task<LearningPathViewModel?> GetLearningPathDetailsAsync(Guid id)
    {
        var path = await learningPathRepository.GetByIdAsync(id);
        if (path == null) return null;

        return new LearningPathViewModel
        {
            Id = path.Id,
            Title = path.Title,
            Description = path.Description,
            Price = path.Price,
            Status = path.Status,
            Courses = path.PathCourses.Select(pc => new CourseViewModel
            {
                Id = pc.Course.Id,
                Title = pc.Course.Title,
                Description = pc.Course.Description,
                Price = pc.Course.Price,
                ImageUrl = pc.Course.ImageUrl,
                InstructorName = pc.Course.Instructor != null ? pc.Course.Instructor.Username : "Unknown",
                CategoryName = pc.Course.Category != null ? pc.Course.Category.Name : "General"
            })
        };
    }

    public async Task<IEnumerable<LearningPathViewModel>> GetFeaturedLearningPathsAsync()
    {
        var paths = await learningPathRepository.GetAllAsync();
        // In a real app, we might filter by "Featured" flag or take top N.
        // For now, return all (or top 3).
        
        return paths.Take(3).Select(path => new LearningPathViewModel
        {
            Id = path.Id,
            Title = path.Title,
            Description = path.Description,
            Price = path.Price,
            Status = path.Status,
            Courses = path.PathCourses.Select(pc => new CourseViewModel
            {
                 Id = pc.Course.Id,
                 Title = pc.Course.Title
                 // Minimal info for listing
            })
        });
    }
}
