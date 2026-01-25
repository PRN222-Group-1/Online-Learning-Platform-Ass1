using Online_Learning_Platform_Ass1.Data.Database.Entities;

namespace Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;

public interface IUserLearningPathEnrollmentRepository
{
    Task<UserLearningPathEnrollment?> GetByIdAsync(Guid enrollmentId);
    Task<UserLearningPathEnrollment?> GetByUserAndPathAsync(Guid userId, Guid pathId);
    Task<IEnumerable<UserLearningPathEnrollment>> GetUserEnrollmentsAsync(Guid userId);
    Task<IEnumerable<UserLearningPathEnrollment>> GetPathEnrollmentsAsync(Guid pathId);
    Task<bool> IsEnrolledAsync(Guid userId, Guid pathId);
    Task AddAsync(UserLearningPathEnrollment enrollment);
    Task UpdateAsync(UserLearningPathEnrollment enrollment);
    Task DeleteAsync(Guid enrollmentId);
    Task SaveChangesAsync();
}
