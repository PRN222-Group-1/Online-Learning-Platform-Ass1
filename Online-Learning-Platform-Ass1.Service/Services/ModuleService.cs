using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.DTOs.Module;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;

public class ModuleService(IModuleRepository moduleRepository) : IModuleService
{
    private readonly IModuleRepository _moduleRepository = moduleRepository;

    public async Task<IEnumerable<ModuleDTO>> GetAllAsync()
    {
        var modules = await _moduleRepository.GetAllAsync();

        return modules.Select(m => new ModuleDTO
        {
            Id = m.Id,
            CourseId = m.CourseId,
            Title = m.Title,
            OrderIndex = m.OrderIndex,
            CreatedAt = m.CreatedAt
        });
    }

    public async Task<ModuleDTO?> GetByIdAsync(int moduleId)
    {
        var m = await _moduleRepository.GetByIdAsync(moduleId);
        if (m == null) return null;

        return new ModuleDTO
        {
            Id = m.Id,
            CourseId = m.CourseId,
            Title = m.Title,
            OrderIndex = m.OrderIndex,
            CreatedAt = m.CreatedAt
        };
    }

    public async Task<IEnumerable<ModuleDTO>> GetByCourseIdAsync(int courseId)
    {
        var modules = await _moduleRepository.GetByCourseIdAsync(courseId);

        return modules.Select(m => new ModuleDTO
        {
            Id = m.Id,
            CourseId = m.CourseId,
            Title = m.Title,
            OrderIndex = m.OrderIndex,
            CreatedAt = m.CreatedAt
        });
    }
}
