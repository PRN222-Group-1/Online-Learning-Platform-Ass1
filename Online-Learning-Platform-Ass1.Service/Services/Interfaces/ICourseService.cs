using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Service.DTOs.Course;

namespace Online_Learning_Platform_Ass1.Service.Services.Interfaces;
public interface ICourseService
{
    Task<IEnumerable<CourseDTO>> GetAllAsync();
    Task<CourseDTO?> GetByIdAsync(int courseId);
}
