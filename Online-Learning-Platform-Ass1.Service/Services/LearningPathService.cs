using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.DTOs.Course;
using Online_Learning_Platform_Ass1.Service.DTOs.LearningPath;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;

public class LearningPathService(
    ILearningPathRepository learningPathRepository,
    IUserLearningPathEnrollmentRepository enrollmentRepository,
    IEnrollmentRepository courseEnrollmentRepository) : ILearningPathService
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
            Courses = path.PathCourses.OrderBy(pc => pc.OrderIndex).Select(pc => new CourseViewModel
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
        var paths = await learningPathRepository.GetFeaturedPathsAsync(5);
        
        return paths.Select(path => new LearningPathViewModel
        {
            Id = path.Id,
            Title = path.Title,
            Description = path.Description,
            Price = path.Price,
            Status = path.Status,
            Courses = path.PathCourses.OrderBy(pc => pc.OrderIndex).Select(pc => new CourseViewModel
            {
                 Id = pc.Course.Id,
                 Title = pc.Course.Title,
                 Description = pc.Course.Description,
                 Price = pc.Course.Price,
                 ImageUrl = pc.Course.ImageUrl,
                 InstructorName = pc.Course.Instructor?.Username ?? "Unknown",
                 CategoryName = pc.Course.Category?.Name ?? "General"
            })
        });
    }

    public async Task<IEnumerable<LearningPathViewModel>> GetPublishedPathsAsync()
    {
        var paths = await learningPathRepository.GetPublishedPathsAsync();
        
        return paths.Select(path => new LearningPathViewModel
        {
            Id = path.Id,
            Title = path.Title,
            Description = path.Description,
            Price = path.Price,
            Status = path.Status,
            Courses = path.PathCourses.OrderBy(pc => pc.OrderIndex).Select(pc => new CourseViewModel
            {
                 Id = pc.Course.Id,
                 Title = pc.Course.Title,
                 Description = pc.Course.Description,
                 Price = pc.Course.Price,
                 ImageUrl = pc.Course.ImageUrl,
                 InstructorName = pc.Course.Instructor?.Username ?? "Unknown",
                 CategoryName = pc.Course.Category?.Name ?? "General"
            })
        });
    }

    public async Task<IEnumerable<UserLearningPathWithProgressDto>> GetUserEnrolledPathsAsync(Guid userId)
    {
        var enrollments = await enrollmentRepository.GetUserEnrollmentsAsync(userId);
        var result = new List<UserLearningPathWithProgressDto>();

        foreach (var enrollment in enrollments)
        {
            var courseEnrollments = await courseEnrollmentRepository.GetStudentEnrollmentsAsync(userId);
            var pathCourseIds = enrollment.LearningPath.PathCourses.Select(pc => pc.CourseId).ToHashSet();
            var completedCount = courseEnrollments.Count(ce => pathCourseIds.Contains(ce.CourseId) && ce.Status == "completed");

            result.Add(new UserLearningPathWithProgressDto
            {
                Id = enrollment.PathId,
                EnrollmentId = enrollment.Id,
                Title = enrollment.LearningPath.Title,
                Description = enrollment.LearningPath.Description,
                Price = enrollment.LearningPath.Price,
                Status = enrollment.LearningPath.Status,
                TotalCourses = enrollment.LearningPath.PathCourses.Count,
                CompletedCourses = completedCount,
                ProgressPercentage = enrollment.ProgressPercentage,
                EnrollmentStatus = enrollment.Status,
                EnrolledAt = enrollment.EnrolledAt,
                CompletedAt = enrollment.CompletedAt,
                Courses = enrollment.LearningPath.PathCourses.OrderBy(pc => pc.OrderIndex).Select(pc => new CourseViewModel
                {
                    Id = pc.Course.Id,
                    Title = pc.Course.Title,
                    Description = pc.Course.Description,
                    Price = pc.Course.Price,
                    ImageUrl = pc.Course.ImageUrl,
                    InstructorName = pc.Course.Instructor?.Username ?? "Unknown",
                    CategoryName = pc.Course.Category?.Name ?? "General"
                })
            });
        }

        return result;
    }

    public async Task<UserLearningPathWithProgressDto?> GetUserPathProgressAsync(Guid userId, Guid pathId)
    {
        var enrollment = await enrollmentRepository.GetByUserAndPathAsync(userId, pathId);
        if (enrollment == null) return null;

        var courseEnrollments = await courseEnrollmentRepository.GetStudentEnrollmentsAsync(userId);
        var pathCourseIds = enrollment.LearningPath.PathCourses.Select(pc => pc.CourseId).ToHashSet();
        var completedCount = courseEnrollments.Count(ce => pathCourseIds.Contains(ce.CourseId) && ce.Status == "completed");

        return new UserLearningPathWithProgressDto
        {
            Id = enrollment.PathId,
            EnrollmentId = enrollment.Id,
            Title = enrollment.LearningPath.Title,
            Description = enrollment.LearningPath.Description,
            Price = enrollment.LearningPath.Price,
            Status = enrollment.LearningPath.Status,
            TotalCourses = enrollment.LearningPath.PathCourses.Count,
            CompletedCourses = completedCount,
            ProgressPercentage = enrollment.ProgressPercentage,
            EnrollmentStatus = enrollment.Status,
            EnrolledAt = enrollment.EnrolledAt,
            CompletedAt = enrollment.CompletedAt,
            Courses = enrollment.LearningPath.PathCourses.OrderBy(pc => pc.OrderIndex).Select(pc => new CourseViewModel
            {
                Id = pc.Course.Id,
                Title = pc.Course.Title,
                Description = pc.Course.Description,
                Price = pc.Course.Price,
                ImageUrl = pc.Course.ImageUrl,
                InstructorName = pc.Course.Instructor?.Username ?? "Unknown",
                CategoryName = pc.Course.Category?.Name ?? "General"
            })
        };
    }

    public async Task<LearningPathDetailsWithProgressDto?> GetPathDetailsWithProgressAsync(Guid pathId, Guid? userId = null)
    {
        var path = await learningPathRepository.GetByIdAsync(pathId);
        if (path == null) return null;

        var dto = new LearningPathDetailsWithProgressDto
        {
            Id = path.Id,
            Title = path.Title,
            Description = path.Description,
            Price = path.Price,
            Status = path.Status,
            TotalCourses = path.PathCourses.Count,
            Courses = path.PathCourses.OrderBy(pc => pc.OrderIndex).Select(pc => new CourseViewModel
            {
                Id = pc.Course.Id,
                Title = pc.Course.Title,
                Description = pc.Course.Description,
                Price = pc.Course.Price,
                ImageUrl = pc.Course.ImageUrl,
                InstructorName = pc.Course.Instructor?.Username ?? "Unknown",
                CategoryName = pc.Course.Category?.Name ?? "General"
            }),
            IsEnrolled = false,
            ProgressPercentage = 0,
            CompletedCourses = 0
        };

        // If userId provided, get enrollment and progress
        if (userId.HasValue)
        {
            var enrollment = await enrollmentRepository.GetByUserAndPathAsync(userId.Value, pathId);
            if (enrollment != null)
            {
                var courseEnrollments = await courseEnrollmentRepository.GetStudentEnrollmentsAsync(userId.Value);
                var pathCourseIds = path.PathCourses.Select(pc => pc.CourseId).ToHashSet();
                var completedCount = courseEnrollments.Count(ce => pathCourseIds.Contains(ce.CourseId) && ce.Status == "completed");

                dto = dto with
                {
                    EnrollmentId = enrollment.Id,
                    IsEnrolled = true,
                    EnrollmentStatus = enrollment.Status,
                    ProgressPercentage = enrollment.ProgressPercentage,
                    CompletedCourses = completedCount,
                    EnrolledAt = enrollment.EnrolledAt,
                    CompletedAt = enrollment.CompletedAt
                };
            }
        }

        return dto;
    }
}
