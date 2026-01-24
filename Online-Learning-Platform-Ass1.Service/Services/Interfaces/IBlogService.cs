using Online_Learning_Platform_Ass1.Service.DTOs.Blog;
using Online_Learning_Platform_Ass1.Service.Results;

namespace Online_Learning_Platform_Ass1.Service.Services.Interfaces;

public interface IBlogService
{
    Task<ServiceResult<IEnumerable<BlogReadDto>>> GetAllPublishedBlogsAsync();
    Task<ServiceResult<IEnumerable<BlogReadDto>>> GetMyBlogsAsync(Guid userId);
    Task<ServiceResult<BlogReadDto>> GetBlogByIdAsync(Guid id);
    Task<ServiceResult<BlogReadDto>> CreateBlogAsync(BlogCreateDto dto, Guid authorId);
    Task<ServiceResult<BlogReadDto>> UpdateBlogAsync(Guid id, BlogUpdateDto dto, Guid userId);
    Task<ServiceResult<bool>> DeleteBlogAsync(Guid id, Guid userId);
}
