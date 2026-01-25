using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.DTOs.Assessment;
using Online_Learning_Platform_Ass1.Service.DTOs.Course;
using Online_Learning_Platform_Ass1.Service.DTOs.LearningPath;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;

public class LearningPathRecommendationService(
    IUserAssessmentRepository assessmentRepository,
    ICourseRepository courseRepository,
    ILearningPathRepository learningPathRepository) : ILearningPathRecommendationService
{
    public async Task<IEnumerable<LearningPathRecommendationDto>> GenerateRecommendationsAsync(Guid assessmentId)
    {
        var assessment = await assessmentRepository.GetAssessmentWithAnswersAsync(assessmentId);
        if (assessment == null)
        {
            return Enumerable.Empty<LearningPathRecommendationDto>();
        }

        // Analyze answers to determine user preferences
        var categoryInterests = new Dictionary<Guid, int>();
        var skillLevelCounts = new Dictionary<string, int>();

        foreach (var answer in assessment.Answers)
        {
            // Count category interests
            if (answer.Question.CategoryId.HasValue)
            {
                var categoryId = answer.Question.CategoryId.Value;
                categoryInterests[categoryId] = categoryInterests.GetValueOrDefault(categoryId, 0) + 1;
            }

            // Count skill levels
            if (!string.IsNullOrEmpty(answer.SelectedOption.SkillLevel))
            {
                var level = answer.SelectedOption.SkillLevel;
                skillLevelCounts[level] = skillLevelCounts.GetValueOrDefault(level, 0) + 1;
            }
        }

        // Determine primary skill level
        var primarySkillLevel = skillLevelCounts.OrderByDescending(kvp => kvp.Value)
            .FirstOrDefault().Key ?? "Beginner";

        // Get all published learning paths
        var allPaths = await learningPathRepository.GetPublishedPathsAsync();
        var allCourses = await courseRepository.GetAllAsync();

        // Generate recommendations based on top categories
        var recommendations = new List<LearningPathRecommendationDto>();
        var topCategories = categoryInterests.OrderByDescending(kvp => kvp.Value).Take(3);

        foreach (var categoryInterest in topCategories)
        {
            var categoryId = categoryInterest.Key;
            
            // Try to find learning paths that contain courses in this category
            var matchingPaths = allPaths
                .Where(p => p.PathCourses.Any(pc => pc.Course.CategoryId == categoryId))
                .Take(3)
                .ToList();

            if (matchingPaths.Any())
            {
                // Return actual learning paths with their courses
                foreach (var path in matchingPaths)
                {
                    var coursesInPath = path.PathCourses
                        .OrderBy(pc => pc.OrderIndex)
                        .Select(pc => new CourseViewModel
                        {
                            Id = pc.Course.Id,
                            Title = pc.Course.Title,
                            Description = pc.Course.Description,
                            Price = pc.Course.Price,
                            ImageUrl = pc.Course.ImageUrl,
                            InstructorName = pc.Course.Instructor?.Username ?? "Unknown",
                            CategoryName = pc.Course.Category?.Name ?? "General"
                        })
                        .ToList();

                    recommendations.Add(new LearningPathRecommendationDto
                    {
                        RecommendationTitle = path.Title,
                        RecommendationReason = $"Recommended path based on your interest in {path.PathCourses.First().Course.Category?.Name}",
                        SkillLevel = primarySkillLevel,
                        RecommendedCourses = coursesInPath,
                        PathId = path.Id,
                        PathPrice = path.Price
                    });
                }
            }
            else
            {
                // Fallback: recommend individual courses in this category
                var categoryCourses = allCourses
                    .Where(c => c.CategoryId == categoryId && c.Status == "published")
                    .Take(5)
                    .Select(c => new CourseViewModel
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Description = c.Description,
                        Price = c.Price,
                        ImageUrl = c.ImageUrl,
                        InstructorName = c.Instructor?.Username ?? "Unknown",
                        CategoryName = c.Category?.Name ?? "General"
                    })
                    .ToList();

                if (categoryCourses.Any())
                {
                    var categoryName = categoryCourses.First().CategoryName;
                    recommendations.Add(new LearningPathRecommendationDto
                    {
                        RecommendationTitle = $"Courses in {categoryName}",
                        RecommendationReason = $"Based on your interest in {categoryName}",
                        SkillLevel = primarySkillLevel,
                        RecommendedCourses = categoryCourses
                    });
                }
            }
        }

        // If no category-based recommendations, provide general paths or courses
        if (!recommendations.Any())
        {
            // Try featured learning paths first
            var featuredPaths = await learningPathRepository.GetFeaturedPathsAsync(3);
            if (featuredPaths.Any())
            {
                foreach (var path in featuredPaths)
                {
                    var coursesInPath = path.PathCourses
                        .OrderBy(pc => pc.OrderIndex)
                        .Select(pc => new CourseViewModel
                        {
                            Id = pc.Course.Id,
                            Title = pc.Course.Title,
                            Description = pc.Course.Description,
                            Price = pc.Course.Price,
                            ImageUrl = pc.Course.ImageUrl,
                            InstructorName = pc.Course.Instructor?.Username ?? "Unknown",
                            CategoryName = pc.Course.Category?.Name ?? "General"
                        })
                        .ToList();

                    recommendations.Add(new LearningPathRecommendationDto
                    {
                        RecommendationTitle = path.Title,
                        RecommendationReason = "Popular path to get started",
                        SkillLevel = primarySkillLevel,
                        RecommendedCourses = coursesInPath,
                        PathId = path.Id,
                        PathPrice = path.Price
                    });
                }
            }
            else
            {
                // Fallback to general courses
                var generalCourses = allCourses
                    .Where(c => c.Status == "published")
                    .Take(5)
                    .Select(c => new CourseViewModel
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Description = c.Description,
                        Price = c.Price,
                        ImageUrl = c.ImageUrl,
                        InstructorName = c.Instructor?.Username ?? "Unknown",
                        CategoryName = c.Category?.Name ?? "General"
                    })
                    .ToList();

                recommendations.Add(new LearningPathRecommendationDto
                {
                    RecommendationTitle = "Explore Our Courses",
                    RecommendationReason = "Popular courses to get started",
                    SkillLevel = primarySkillLevel,
                    RecommendedCourses = generalCourses
                });
            }
        }

        return recommendations;
    }
}
