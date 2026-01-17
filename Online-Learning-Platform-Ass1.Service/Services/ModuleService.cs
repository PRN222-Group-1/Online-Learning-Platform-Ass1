using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.DTOs.Module;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;
public class ModuleService(IModuleRepository moduleRepository) : IModuleService
{
    public List<ModuleDTO> GetModulesByCourseId(int courseId)
    {
        return moduleRepository.GetModulesByCourseId(courseId)
            .Select(m => new ModuleDTO(m.Id, m.CourseId, m.Title, m.OrderIndex, m.CreatedAt))
            .ToList();
    }
}
