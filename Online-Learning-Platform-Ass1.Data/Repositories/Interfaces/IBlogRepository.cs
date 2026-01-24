using Online_Learning_Platform_Ass1.Data.Database.Entities;

namespace Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;

public interface IBlogRepository
{
    Task<IEnumerable<Blog>> GetAllPublishedAsync();
    Task<IEnumerable<Blog>> GetByAuthorIdAsync(Guid authorId);
    Task<Blog?> GetByIdAsync(Guid id);
    Task<Blog> CreateAsync(Blog blog);
    Task<Blog> UpdateAsync(Blog blog);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
