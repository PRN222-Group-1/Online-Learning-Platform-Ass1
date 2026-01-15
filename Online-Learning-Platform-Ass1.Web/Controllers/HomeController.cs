using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Online_Learning_Platform_Ass1.Data.Models;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;
using Online_Learning_Platform_Ass1.Web.Models;
using Online_Learning_Platform_Ass1.Service.DTOs.Course;

namespace Online_Learning_Platform_Ass1.Data.Controllers;

public class HomeController(
    ICourseService courseService,
    ILearningPathService learningPathService) : Controller
{
    public async Task<IActionResult> IndexAsync(string? searchTerm = null, Guid? categoryId = null)
    {
        // If searching or filtering, get filtered list. Otherwise get featured.
        // Actually, user wants "Category ... shows corresponding courses".
        // Let's interpret: 
        // Default: Featured Courses + Featured Paths.
        // Filtered: Filtered Courses (hide paths? or show all?). Let's hide paths if searching courses.
        
        IEnumerable<CourseViewModel> courses;
        if (!string.IsNullOrEmpty(searchTerm) || categoryId.HasValue)
        {
            courses = await courseService.GetAllCoursesAsync(searchTerm, categoryId);
        }
        else
        {
            courses = await courseService.GetFeaturedCoursesAsync();
        }

        var paths = await learningPathService.GetFeaturedLearningPathsAsync();
        var categories = await courseService.GetAllCategoriesAsync();

        var model = new HomeViewModel
        {
            FeaturedCourses = courses,
            FeaturedPaths = paths,
            SearchTerm = searchTerm,
            SelectedCategoryId = categoryId,
            Categories = categories
        };
        return View(model);
    }

    public IActionResult PrivacyAsync() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult ErrorAsync() => View(new ErrorViewModel
        { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
