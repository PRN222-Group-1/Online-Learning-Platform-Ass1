using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Service.DTOs.Module;

namespace Online_Learning_Platform_Ass1.Service.Services.Interfaces;
public interface IModuleService
{
    public List<ModuleDTO> GetModulesByCourseId(int courseId);
}
