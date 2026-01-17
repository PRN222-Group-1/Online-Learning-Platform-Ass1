using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Service.DTOs.Lesson;

namespace Online_Learning_Platform_Ass1.Service.Services.Interfaces;
public interface ILessonService
{
    public List<LessonDTO> GetLessonsByModuleId(int moduleId);
}
