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
    private readonly List<Course> _courses = new();
    private int _currentId = 1;

    public CourseRepository()
    {

        _courses.Add(new Course
        {
            Id = _currentId++,
            Author = "Nguyễn Văn A",
            Title = "Lập trình ASP.NET Core từ A đến Z",
            Description = "Khóa học toàn diện về ASP.NET Core, bao gồm MVC, Web API, Entity Framework Core, và nhiều hơn nữa.",
            PictureUrl = "https://online-learning-platform.sfo3.cdn.digitaloceanspaces.com/course1.jpg",
            CreatedAt = DateTime.UtcNow
        });

        _courses.Add(new Course
        {
            Id = _currentId++,
            Author = "Trần Thị B",
            Title = "Xây dựng ứng dụng web với Blazor",
            Description = "Học cách xây dựng ứng dụng web tương tác sử dụng Blazor Server và Blazor WebAssembly.",
            PictureUrl = "https://online-learning-platform.sfo3.cdn.digitaloceanspaces.com/course2.jpg",
            CreatedAt = DateTime.UtcNow
        });

    }


    public Task<IEnumerable<Course>> GetAllAsync()
    {
        return Task.FromResult(_courses.AsEnumerable());
    }

    public Task<Course?> GetByIdAsync(int courseId)
    {
        var course = _courses.FirstOrDefault(c => c.Id == courseId);
        return Task.FromResult(course);
    }

    public Task AddAsync(Course course)
    {
        course.Id = _currentId++;
        _courses.Add(course);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Course course)
    {
        var existing = _courses.FirstOrDefault(c => c.Id == course.Id);
        if (existing == null) return Task.CompletedTask;

        existing.Title = course.Title;
        existing.Description = course.Description;
        existing.Modules = course.Modules;

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int courseId)
    {
        var course = _courses.FirstOrDefault(c => c.Id == courseId);
        if (course != null)
        {
            _courses.Remove(course);
        }
        return Task.CompletedTask;
    }
}
