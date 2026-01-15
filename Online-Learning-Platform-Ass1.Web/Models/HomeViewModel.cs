using Online_Learning_Platform_Ass1.Service.DTOs.Category;
using Online_Learning_Platform_Ass1.Service.DTOs.Course;
using Online_Learning_Platform_Ass1.Service.DTOs.LearningPath;

namespace Online_Learning_Platform_Ass1.Web.Models;

public class HomeViewModel
{
    public IEnumerable<CourseViewModel> FeaturedCourses { get; set; } = new List<CourseViewModel>();
    public IEnumerable<LearningPathViewModel> FeaturedPaths { get; set; } = new List<LearningPathViewModel>();
    
    // Search & Filter
    public string? SearchTerm { get; set; }
    public Guid? SelectedCategoryId { get; set; }
    public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
}
