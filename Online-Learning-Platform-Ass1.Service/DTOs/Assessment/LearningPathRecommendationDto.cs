using Online_Learning_Platform_Ass1.Service.DTOs.Course;

namespace Online_Learning_Platform_Ass1.Service.DTOs.Assessment;

public class LearningPathRecommendationDto
{
    public string RecommendationTitle { get; set; } = null!;
    public string RecommendationReason { get; set; } = null!;
    public string SkillLevel { get; set; } = null!;
    public List<CourseViewModel> RecommendedCourses { get; set; } = new();
    
    // Learning Path info (if this recommendation is for a full path)
    public Guid? PathId { get; set; }
    public decimal? PathPrice { get; set; }
}
