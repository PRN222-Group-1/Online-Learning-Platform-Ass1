using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.DTOs.Course;
using Online_Learning_Platform_Ass1.Service.DTOs.LearningPath;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;

public class LearningPathEnrollmentService(
    IUserLearningPathEnrollmentRepository enrollmentRepository,
    ILearningPathRepository pathRepository,
    IEnrollmentRepository courseEnrollmentRepository) : ILearningPathEnrollmentService
{
    public async Task<UserLearningPathEnrollmentDto?> EnrollUserAsync(Guid userId, Guid pathId)
    {
        // Check if already enrolled
        var existing = await enrollmentRepository.GetByUserAndPathAsync(userId, pathId);
        if (existing != null)
        {
            return MapToDto(existing);
        }

        // Create new enrollment
        var enrollment = new UserLearningPathEnrollment
        {
            UserId = userId,
            PathId = pathId,
            Status = "active",
            ProgressPercentage = 0
        };

        await enrollmentRepository.AddAsync(enrollment);

        return MapToDto(enrollment);
    }

    public async Task<UserLearningPathEnrollmentDto?> GetEnrollmentAsync(Guid enrollmentId)
    {
        var enrollment = await enrollmentRepository.GetByIdAsync(enrollmentId);
        if (enrollment == null) return null;

        return MapToDto(enrollment);
    }

    public async Task<IEnumerable<UserLearningPathEnrollmentDto>> GetUserEnrollmentsAsync(Guid userId)
    {
        var enrollments = await enrollmentRepository.GetUserEnrollmentsAsync(userId);
        return enrollments.Select(MapToDto).ToList();
    }

    public async Task<bool> IsEnrolledAsync(Guid userId, Guid pathId)
    {
        return await enrollmentRepository.IsEnrolledAsync(userId, pathId);
    }

    public async Task<bool> UpdateEnrollmentStatusAsync(Guid enrollmentId, string status)
    {
        var enrollment = await enrollmentRepository.GetByIdAsync(enrollmentId);
        if (enrollment == null) return false;

        enrollment.Status = status;
        await enrollmentRepository.UpdateAsync(enrollment);
        return true;
    }

    public async Task<int> CalculateProgressAsync(Guid userId, Guid pathId)
    {
        var path = await pathRepository.GetByIdAsync(pathId);
        if (path == null || !path.PathCourses.Any())
            return 0;

        var courseIds = path.PathCourses.Select(pc => pc.CourseId).ToList();
        int completedCount = 0;

        // Check how many courses the user has completed
        foreach (var courseId in courseIds)
        {
            // Check if user has completed the course enrollment
            var enrollment = await courseEnrollmentRepository.GetStudentEnrollmentsAsync(userId);
            var isCompleted = enrollment.Any(e => e.CourseId == courseId && e.Status == "completed");
            if (isCompleted)
            {
                completedCount++;
            }
        }

        // Calculate percentage
        int percentage = courseIds.Count > 0 
            ? (int)Math.Round((decimal)completedCount / courseIds.Count * 100) 
            : 0;

        // Update the enrollment progress
        var pathEnrollment = await enrollmentRepository.GetByUserAndPathAsync(userId, pathId);
        if (pathEnrollment != null)
        {
            pathEnrollment.ProgressPercentage = percentage;
            await enrollmentRepository.UpdateAsync(pathEnrollment);
        }

        return percentage;
    }

    public async Task<bool> CompletePathAsync(Guid enrollmentId)
    {
        var enrollment = await enrollmentRepository.GetByIdAsync(enrollmentId);
        if (enrollment == null) return false;

        enrollment.Status = "completed";
        enrollment.CompletedAt = DateTime.UtcNow;
        enrollment.ProgressPercentage = 100;

        await enrollmentRepository.UpdateAsync(enrollment);
        return true;
    }

    public async Task<bool> DropPathAsync(Guid enrollmentId)
    {
        var enrollment = await enrollmentRepository.GetByIdAsync(enrollmentId);
        if (enrollment == null) return false;

        enrollment.Status = "dropped";
        await enrollmentRepository.UpdateAsync(enrollment);
        return true;
    }

    public async Task<UserLearningPathEnrollmentWithDetailsDto?> GetEnrollmentDetailsAsync(Guid enrollmentId)
    {
        var enrollment = await enrollmentRepository.GetByIdAsync(enrollmentId);
        if (enrollment == null) return null;

        var courseEnrollments = await courseEnrollmentRepository.GetStudentEnrollmentsAsync(enrollment.UserId);
        var pathCourseIds = enrollment.LearningPath.PathCourses.Select(pc => pc.CourseId).ToHashSet();
        
        var completedCourses = courseEnrollments
            .Count(ce => pathCourseIds.Contains(ce.CourseId) && ce.Status == "completed");

        var courses = enrollment.LearningPath.PathCourses
            .Select(pc => new CourseViewModel
            {
                Id = pc.Course.Id,
                Title = pc.Course.Title,
                Description = pc.Course.Description,
                Price = pc.Course.Price,
                ImageUrl = pc.Course.ImageUrl,
                InstructorName = pc.Course.Instructor?.Username ?? "Unknown",
                CategoryName = pc.Course.Category?.Name ?? "General"
            })
            .ToList();

        return new UserLearningPathEnrollmentWithDetailsDto
        {
            Id = enrollment.Id,
            UserId = enrollment.UserId,
            PathId = enrollment.PathId,
            EnrolledAt = enrollment.EnrolledAt,
            CompletedAt = enrollment.CompletedAt,
            Status = enrollment.Status,
            ProgressPercentage = enrollment.ProgressPercentage,
            PathTitle = enrollment.LearningPath.Title,
            PathDescription = enrollment.LearningPath.Description,
            PathPrice = enrollment.LearningPath.Price,
            TotalCourses = enrollment.LearningPath.PathCourses.Count,
            CompletedCourses = completedCourses,
            Courses = courses
        };
    }

    private static UserLearningPathEnrollmentDto MapToDto(UserLearningPathEnrollment enrollment)
    {
        return new UserLearningPathEnrollmentDto
        {
            Id = enrollment.Id,
            UserId = enrollment.UserId,
            PathId = enrollment.PathId,
            EnrolledAt = enrollment.EnrolledAt,
            CompletedAt = enrollment.CompletedAt,
            Status = enrollment.Status,
            ProgressPercentage = enrollment.ProgressPercentage
        };
    }
}
