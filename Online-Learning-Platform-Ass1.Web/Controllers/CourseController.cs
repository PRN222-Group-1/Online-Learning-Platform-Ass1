using Online_Learning_Platform_Ass1.Data.Models;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Online_Learning_Platform_Ass1.Data.Controllers;
public class CourseController(ICourseService courseService, IModuleService moduleService, ILessonService lessonService) : Controller
{
    private readonly ICourseService _courseService = courseService;
    private readonly IModuleService _moduleService = moduleService;
    private readonly ILessonService _lessonService = lessonService;

    // ví dụ như /Course/Learn/5?lessonId=12
    [HttpGet]
    public async Task<IActionResult> Learn(int courseId, int? lessonId)
    {
        var course = await _courseService.GetByIdAsync(courseId);
        if (course == null)
            return NotFound();

        var modules = await _moduleService.GetByCourseIdAsync(courseId);

        var vm = new CourseViewModel
        {
            CourseId = course.Id,
            Title = course.Title,
            Description = course.Description,
            CurrentLessonId = lessonId
        };

        foreach (var module in modules)
        {
            var moduleVm = new ModuleViewModel
            {
                ModuleId = module.Id,
                Title = module.Title
            };

            var lessons = await _lessonService.GetByModuleIdAsync(module.Id);

            foreach (var lesson in lessons)
            {
                moduleVm.Lessons.Add(new LessonViewModel
                {
                    LessonId = lesson.Id,
                    Title = lesson.Title,
                    IsCurrent = lesson.Id == lessonId
                });
            }

            vm.Modules.Add(moduleVm);
        }

        return View(vm);
    }

    public async Task<IActionResult> List()
    {
        var courses = await _courseService.GetAllAsync();
        return View(courses);
    }
}
