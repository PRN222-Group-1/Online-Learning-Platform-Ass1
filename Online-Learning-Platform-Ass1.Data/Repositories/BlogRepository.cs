using Microsoft.EntityFrameworkCore;
using Online_Learning_Platform_Ass1.Data.Database;
using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;

namespace Online_Learning_Platform_Ass1.Data.Repositories;

public class BlogRepository : IBlogRepository
{
    private readonly OnlineLearningContext _context;

    public BlogRepository(OnlineLearningContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Blog>> GetAllPublishedAsync()
    {
        return await _context.Blogs
            .Include(b => b.Author)
            .Where(b => b.Status == "published")
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Blog>> GetByAuthorIdAsync(Guid authorId)
    {
        return await _context.Blogs
            .Include(b => b.Author)
            .Where(b => b.AuthorId == authorId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<Blog?> GetByIdAsync(Guid id)
    {
        return await _context.Blogs
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Blog> CreateAsync(Blog blog)
    {
        blog.Id = Guid.NewGuid();
        blog.CreatedAt = DateTime.UtcNow;
        
        await _context.Blogs.AddAsync(blog);
        await _context.SaveChangesAsync();
        
        return blog;
    }

    public async Task<Blog> UpdateAsync(Blog blog)
    {
        _context.Blogs.Update(blog);
        await _context.SaveChangesAsync();
        
        return blog;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var blog = await _context.Blogs.FindAsync(id);
        if (blog == null)
            return false;

        _context.Blogs.Remove(blog);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Blogs.AnyAsync(b => b.Id == id);
    }
}
