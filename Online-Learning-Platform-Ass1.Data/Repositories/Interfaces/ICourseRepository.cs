using Online_Learning_Platform_Ass1.Data.Database.Entities;

namespace Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(Guid id);
    Task<IEnumerable<Course>> GetAllAsync();
    Task<IEnumerable<Course>> GetFeaturableCoursesAsync(int count);
    Task<IEnumerable<Course>> GetCoursesAsync(string? searchTerm = null, Guid? categoryId = null);
    Task<int> SaveChangesAsync();
}
