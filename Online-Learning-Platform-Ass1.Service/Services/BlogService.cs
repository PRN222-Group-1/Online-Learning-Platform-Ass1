using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.DTOs.Blog;
using Online_Learning_Platform_Ass1.Service.Results;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;

public class BlogService : IBlogService
{
    private readonly IBlogRepository _blogRepository;
    private readonly IUserRepository _userRepository;

    public BlogService(IBlogRepository blogRepository, IUserRepository userRepository)
    {
        _blogRepository = blogRepository;
        _userRepository = userRepository;
    }

    public async Task<ServiceResult<IEnumerable<BlogReadDto>>> GetAllPublishedBlogsAsync()
    {
        var blogs = await _blogRepository.GetAllPublishedAsync();
        var blogDtos = blogs.Select(MapToReadDto);
        
        return ServiceResult<IEnumerable<BlogReadDto>>.SuccessResult(blogDtos);
    }

    public async Task<ServiceResult<IEnumerable<BlogReadDto>>> GetMyBlogsAsync(Guid userId)
    {
        var blogs = await _blogRepository.GetByAuthorIdAsync(userId);
        var blogDtos = blogs.Select(MapToReadDto);
        
        return ServiceResult<IEnumerable<BlogReadDto>>.SuccessResult(blogDtos);
    }

    public async Task<ServiceResult<BlogReadDto>> GetBlogByIdAsync(Guid id)
    {
        var blog = await _blogRepository.GetByIdAsync(id);
        
        if (blog == null)
            return ServiceResult<BlogReadDto>.FailureResult("Blog not found");
        
        return ServiceResult<BlogReadDto>.SuccessResult(MapToReadDto(blog));
    }

    public async Task<ServiceResult<BlogReadDto>> CreateBlogAsync(BlogCreateDto dto, Guid authorId)
    {
        // Verify user exists
        var user = await _userRepository.GetByIdAsync(authorId);
        if (user == null)
            return ServiceResult<BlogReadDto>.FailureResult("User not found");

        // Check if user is Teacher or Admin
        if (user.Role?.Name != "Teacher" && user.Role?.Name != "Admin")
            return ServiceResult<BlogReadDto>.FailureResult("Only Teachers and Admins can create blogs");

        var blog = new Blog
        {
            AuthorId = authorId,
            Title = dto.Title,
            Content = dto.Content,
            Status = dto.Status
        };

        var createdBlog = await _blogRepository.CreateAsync(blog);
        
        // Reload to get author info
        var blogWithAuthor = await _blogRepository.GetByIdAsync(createdBlog.Id);
        
        return ServiceResult<BlogReadDto>.SuccessResult(MapToReadDto(blogWithAuthor!));
    }

    public async Task<ServiceResult<BlogReadDto>> UpdateBlogAsync(Guid id, BlogUpdateDto dto, Guid userId)
    {
        var blog = await _blogRepository.GetByIdAsync(id);
        
        if (blog == null)
            return ServiceResult<BlogReadDto>.FailureResult("Blog not found");

        // Check if user is the author
        if (blog.AuthorId != userId)
            return ServiceResult<BlogReadDto>.FailureResult("You can only edit your own blogs");

        blog.Title = dto.Title;
        blog.Content = dto.Content;
        blog.Status = dto.Status;

        var updatedBlog = await _blogRepository.UpdateAsync(blog);
        
        return ServiceResult<BlogReadDto>.SuccessResult(MapToReadDto(updatedBlog));
    }

    public async Task<ServiceResult<bool>> DeleteBlogAsync(Guid id, Guid userId)
    {
        var blog = await _blogRepository.GetByIdAsync(id);
        
        if (blog == null)
            return ServiceResult<bool>.FailureResult("Blog not found");

        // Check if user is the author
        if (blog.AuthorId != userId)
            return ServiceResult<bool>.FailureResult("You can only delete your own blogs");

        var deleted = await _blogRepository.DeleteAsync(id);
        
        return ServiceResult<bool>.SuccessResult(deleted);
    }

    private static BlogReadDto MapToReadDto(Blog blog)
    {
        return new BlogReadDto
        {
            Id = blog.Id,
            AuthorId = blog.AuthorId,
            AuthorName = blog.Author?.Username ?? "Unknown",
            Title = blog.Title,
            Content = blog.Content,
            Status = blog.Status,
            CreatedAt = blog.CreatedAt
        };
    }
}
