using Microsoft.EntityFrameworkCore;
using Online_Learning_Platform_Ass1.Data.Database;
using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;

namespace Online_Learning_Platform_Ass1.Data.Repositories;

public class UserLearningPathEnrollmentRepository(OnlineLearningContext context) : IUserLearningPathEnrollmentRepository
{
    public async Task<UserLearningPathEnrollment?> GetByIdAsync(Guid enrollmentId)
    {
        return await context.UserLearningPathEnrollments
            .AsNoTracking()
            .Include(ulpe => ulpe.User)
            .Include(ulpe => ulpe.LearningPath)
                .ThenInclude(lp => lp.PathCourses)
                    .ThenInclude(pc => pc.Course)
            .FirstOrDefaultAsync(ulpe => ulpe.Id == enrollmentId);
    }

    public async Task<UserLearningPathEnrollment?> GetByUserAndPathAsync(Guid userId, Guid pathId)
    {
        return await context.UserLearningPathEnrollments
            .AsNoTracking()
            .Include(ulpe => ulpe.LearningPath)
                .ThenInclude(lp => lp.PathCourses)
                    .ThenInclude(pc => pc.Course)
            .FirstOrDefaultAsync(ulpe => ulpe.UserId == userId && ulpe.PathId == pathId);
    }

    public async Task<IEnumerable<UserLearningPathEnrollment>> GetUserEnrollmentsAsync(Guid userId)
    {
        return await context.UserLearningPathEnrollments
            .AsNoTracking()
            .Include(ulpe => ulpe.LearningPath)
                .ThenInclude(lp => lp.PathCourses)
                    .ThenInclude(pc => pc.Course)
            .Where(ulpe => ulpe.UserId == userId && ulpe.Status != "dropped")
            .OrderByDescending(ulpe => ulpe.EnrolledAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserLearningPathEnrollment>> GetPathEnrollmentsAsync(Guid pathId)
    {
        return await context.UserLearningPathEnrollments
            .AsNoTracking()
            .Include(ulpe => ulpe.User)
            .Where(ulpe => ulpe.PathId == pathId && ulpe.Status != "dropped")
            .OrderByDescending(ulpe => ulpe.EnrolledAt)
            .ToListAsync();
    }

    public async Task<bool> IsEnrolledAsync(Guid userId, Guid pathId)
    {
        return await context.UserLearningPathEnrollments
            .AnyAsync(ulpe => ulpe.UserId == userId 
                && ulpe.PathId == pathId 
                && ulpe.Status == "active");
    }

    public async Task AddAsync(UserLearningPathEnrollment enrollment)
    {
        enrollment.Id = Guid.NewGuid();
        enrollment.EnrolledAt = DateTime.UtcNow;
        await context.UserLearningPathEnrollments.AddAsync(enrollment);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserLearningPathEnrollment enrollment)
    {
        context.UserLearningPathEnrollments.Update(enrollment);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid enrollmentId)
    {
        var enrollment = await context.UserLearningPathEnrollments.FindAsync(enrollmentId);
        if (enrollment != null)
        {
            context.UserLearningPathEnrollments.Remove(enrollment);
            await context.SaveChangesAsync();
        }
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
