using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Data.Database.Entities;

namespace Online_Learning_Platform_Ass1.Data.Repositories;
public class CourseRepository : ICourseRepository
{
    List<Course> courses = new List<Course>();

    public CourseRepository()
    {
        courses.Add(new Course { Id = 1, Title = "Course 1", Description = "Description for Course 1", CreatedAt = DateTime.Now });
        courses.Add(new Course { Id = 2, Title = "Course 2", Description = "Description for Course 2", CreatedAt = DateTime.Now });
        courses.Add(new Course { Id = 3, Title = "Course 3", Description = "Description for Course 3", CreatedAt = DateTime.Now });
    }
}
