using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;

namespace Online_Learning_Platform_Ass1.Data.Repositories;
public class ModuleRepository : IModuleRepository
{
    public List<CourseModule> courseModules = new List<CourseModule>();
    public ModuleRepository()
    {
        courseModules.Add(new CourseModule { Id = 1, Title = "Module 1", CourseId = 1, OrderIndex = 1, CreatedAt = DateTime.Now });
        courseModules.Add(new CourseModule { Id = 2, Title = "Module 2", CourseId = 1, OrderIndex = 2, CreatedAt = DateTime.Now });
        courseModules.Add(new CourseModule { Id = 3, Title = "Module 3", CourseId = 1, OrderIndex = 3, CreatedAt = DateTime.Now });

    }
    public List<CourseModule> GetModulesByCourseId(int courseId)
    {
        return courseModules
            .Where(c => c.CourseId == courseId)
            .OrderBy(c => c.OrderIndex)
            .ToList();
    }
}
