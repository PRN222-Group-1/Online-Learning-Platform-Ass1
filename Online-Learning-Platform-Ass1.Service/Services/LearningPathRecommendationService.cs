using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.DTOs.Assessment;
using Online_Learning_Platform_Ass1.Service.DTOs.Course;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;

public class LearningPathRecommendationService(
    IUserAssessmentRepository assessmentRepository,
    ICourseRepository courseRepository,
    ILearningPathRepository learningPathRepository,
    ICategoryRepository categoryRepository,
    IUserLearningPathEnrollmentRepository enrollmentRepository) : ILearningPathRecommendationService
{
    public async Task<IEnumerable<LearningPathRecommendationDto>> GenerateRecommendationsAsync(Guid assessmentId)
    {
        var assessment = await assessmentRepository.GetAssessmentWithAnswersAsync(assessmentId);
        if (assessment == null)
        {
            return Enumerable.Empty<LearningPathRecommendationDto>();
        }

        // Check if we already generated paths for this assessment
        var existingPath = await learningPathRepository.GetByAssessmentIdAsync(assessmentId);
        if (existingPath != null)
        {
            // Return existing recommendation
            return new List<LearningPathRecommendationDto>
            {
                ConvertToDto(existingPath, true)
            };
        }

        // Step 1: Analyze user's answers to build a profile
        var userProfile = AnalyzeUserProfile(assessment);

        // Step 2: Get all available courses
        var allCourses = (await courseRepository.GetAllAsync())
            .Where(c => c.Status == "published" || c.Status == "active")
            .ToList();

        // Step 3: Get all categories for reference
        var allCategories = await categoryRepository.GetAllCategoriesAsync();

        // Step 4: Generate and SAVE personalized learning paths
        var recommendations = new List<LearningPathRecommendationDto>();

        // Generate a custom learning path for the top category the user is interested in
        var topCategory = userProfile.CategoryInterests.OrderByDescending(c => c.Value).FirstOrDefault();
        if (topCategory.Key != Guid.Empty)
        {
            var category = allCategories.FirstOrDefault(c => c.Id == topCategory.Key);
            if (category != null)
            {
                var customPath = await GenerateAndSaveCustomLearningPath(
                    assessment.UserId,
                    assessmentId,
                    category,
                    userProfile,
                    allCourses
                );

                if (customPath != null)
                {
                    recommendations.Add(customPath);
                }
            }
        }

        // If no category-specific recommendations, create a general "Getting Started" path
        if (!recommendations.Any())
        {
            var generalPath = await GenerateAndSaveGeneralLearningPath(
                assessment.UserId,
                assessmentId,
                userProfile, 
                allCourses, 
                allCategories
            );
            if (generalPath != null)
            {
                recommendations.Add(generalPath);
            }
        }

        // Also suggest matching pre-built learning paths (but don't save these)
        var matchingPaths = await FindMatchingPrebuiltPaths(userProfile);
        recommendations.AddRange(matchingPaths);

        return recommendations.Take(5);
    }

    private LearningPathRecommendationDto ConvertToDto(LearningPath path, bool isCustom)
    {
        return new LearningPathRecommendationDto
        {
            RecommendationTitle = path.Title,
            RecommendationReason = path.Description ?? "Your personalized learning path",
            SkillLevel = "Personalized",
            RecommendedCourses = path.PathCourses
                .OrderBy(pc => pc.OrderIndex)
                .Select((pc, index) => new CourseViewModel
                {
                    Id = pc.Course.Id,
                    Title = pc.Course.Title,
                    Description = pc.Course.Description,
                    Price = pc.Course.Price,
                    ImageUrl = pc.Course.ImageUrl,
                    InstructorName = pc.Course.Instructor?.Username ?? "Unknown",
                    CategoryName = pc.Course.Category?.Name ?? "General",
                    OrderIndex = index + 1
                }).ToList(),
            PathId = path.Id,
            PathPrice = path.Price
        };
    }

    private UserAssessmentProfile AnalyzeUserProfile(UserAssessment assessment)
    {
        var profile = new UserAssessmentProfile();

        foreach (var answer in assessment.Answers)
        {
            // Track category interests
            if (answer.Question.CategoryId.HasValue)
            {
                var categoryId = answer.Question.CategoryId.Value;
                profile.CategoryInterests[categoryId] = 
                    profile.CategoryInterests.GetValueOrDefault(categoryId, 0) + 1;
            }

            // Track skill levels
            if (!string.IsNullOrEmpty(answer.SelectedOption.SkillLevel))
            {
                var level = answer.SelectedOption.SkillLevel.ToLower();
                profile.SkillLevelScores[level] = 
                    profile.SkillLevelScores.GetValueOrDefault(level, 0) + 1;
            }

            // Analyze question types for additional context
            switch (answer.Question.QuestionType.ToLower())
            {
                case "interest":
                    profile.InterestIndicators.Add(answer.SelectedOption.OptionText);
                    break;
                case "experience":
                    profile.ExperienceIndicators.Add(answer.SelectedOption.OptionText);
                    break;
                case "goal":
                    profile.GoalIndicators.Add(answer.SelectedOption.OptionText);
                    break;
            }
        }

        // Determine primary skill level
        profile.PrimarySkillLevel = profile.SkillLevelScores
            .OrderByDescending(kvp => kvp.Value)
            .FirstOrDefault().Key ?? "beginner";

        return profile;
    }

    private async Task<LearningPathRecommendationDto?> GenerateAndSaveCustomLearningPath(
        Guid userId,
        Guid assessmentId,
        Category category,
        UserAssessmentProfile profile,
        List<Course> allCourses)
    {
        // Filter courses by category
        var categoryCourses = allCourses
            .Where(c => c.CategoryId == category.Id)
            .ToList();

        if (!categoryCourses.Any()) return null;

        // Score and sort courses based on user profile
        var scoredCourses = categoryCourses
            .Select(course => new
            {
                Course = course,
                Score = CalculateCourseRelevanceScore(course, profile)
            })
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.Course.Price)
            .Take(5)
            .ToList();

        if (!scoredCourses.Any()) return null;

        var selectedCourses = scoredCourses.Select(x => x.Course).ToList();
        var totalPrice = selectedCourses.Sum(c => c.Price);
        var discountedPrice = totalPrice * 0.85m; // 15% bundle discount

        // Generate title and description
        var (title, reason) = GeneratePathTitleAndReason(category.Name, profile, selectedCourses.Count);

        // Create and save the learning path
        var learningPath = new LearningPath
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = reason,
            Price = discountedPrice,
            Status = "published",
            IsCustomPath = true,
            CreatedByUserId = userId,
            SourceAssessmentId = assessmentId,
            CreatedAt = DateTime.UtcNow
        };

        await learningPathRepository.AddAsync(learningPath);

        // Add courses to the path
        for (int i = 0; i < selectedCourses.Count; i++)
        {
            var pathCourse = new PathCourse
            {
                PathId = learningPath.Id,
                CourseId = selectedCourses[i].Id,
                OrderIndex = i + 1
            };
            await learningPathRepository.AddPathCourseAsync(pathCourse);
        }

        // Auto-enroll user in this path
        var enrollment = new UserLearningPathEnrollment
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            PathId = learningPath.Id,
            Status = "active",
            ProgressPercentage = 0,
            EnrolledAt = DateTime.UtcNow
        };
        await enrollmentRepository.AddAsync(enrollment);

        await learningPathRepository.SaveChangesAsync();

        return new LearningPathRecommendationDto
        {
            RecommendationTitle = title,
            RecommendationReason = reason,
            SkillLevel = char.ToUpper(profile.PrimarySkillLevel[0]) + profile.PrimarySkillLevel[1..],
            RecommendedCourses = selectedCourses.Select((c, index) => new CourseViewModel
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Price = c.Price,
                ImageUrl = c.ImageUrl,
                InstructorName = c.Instructor?.Username ?? "Unknown",
                CategoryName = category.Name,
                OrderIndex = index + 1
            }).ToList(),
            PathId = learningPath.Id,
            PathPrice = discountedPrice
        };
    }

    private double CalculateCourseRelevanceScore(Course course, UserAssessmentProfile profile)
    {
        double score = 0;

        // Base score for being in a category of interest
        if (profile.CategoryInterests.ContainsKey(course.CategoryId))
        {
            score += profile.CategoryInterests[course.CategoryId] * 10;
        }

        // Score based on skill level matching (from title/description keywords)
        var courseText = $"{course.Title} {course.Description}".ToLower();
        
        switch (profile.PrimarySkillLevel)
        {
            case "beginner":
                if (courseText.Contains("beginner") || courseText.Contains("introduction") || 
                    courseText.Contains("fundamental") || courseText.Contains("basic") ||
                    courseText.Contains("getting started"))
                    score += 20;
                break;
            case "intermediate":
                if (courseText.Contains("intermediate") || courseText.Contains("practical") ||
                    courseText.Contains("hands-on") || courseText.Contains("project"))
                    score += 20;
                break;
            case "advanced":
                if (courseText.Contains("advanced") || courseText.Contains("expert") ||
                    courseText.Contains("master") || courseText.Contains("professional"))
                    score += 20;
                break;
        }

        // Bonus for matching interest/goal keywords
        foreach (var interest in profile.InterestIndicators)
        {
            if (courseText.Contains(interest.ToLower()))
                score += 5;
        }

        foreach (var goal in profile.GoalIndicators)
        {
            if (courseText.Contains(goal.ToLower()))
                score += 5;
        }

        // Slight preference for newer courses
        var ageInDays = (DateTime.UtcNow - course.CreatedAt).TotalDays;
        if (ageInDays < 30) score += 5;
        else if (ageInDays < 90) score += 3;

        return score;
    }

    private (string Title, string Reason) GeneratePathTitleAndReason(
        string categoryName, 
        UserAssessmentProfile profile,
        int courseCount)
    {
        var levelText = profile.PrimarySkillLevel switch
        {
            "beginner" => "Getting Started with",
            "intermediate" => "Advancing Your Skills in",
            "advanced" => "Mastering",
            _ => "Learning"
        };

        var title = $"{levelText} {categoryName}";
        
        var reason = profile.PrimarySkillLevel switch
        {
            "beginner" => $"A personalized {courseCount}-course path designed for beginners. Start from the fundamentals and build a strong foundation in {categoryName}.",
            "intermediate" => $"A customized {courseCount}-course path to take your {categoryName} skills to the next level with practical, hands-on learning.",
            "advanced" => $"An advanced {courseCount}-course path for experienced learners ready to master {categoryName} concepts.",
            _ => $"A personalized {courseCount}-course learning path tailored to your interests in {categoryName}."
        };

        return (title, reason);
    }

    private async Task<LearningPathRecommendationDto?> GenerateAndSaveGeneralLearningPath(
        Guid userId,
        Guid assessmentId,
        UserAssessmentProfile profile,
        List<Course> allCourses,
        IEnumerable<Category> categories)
    {
        // Select top courses across all categories
        var topCourses = allCourses
            .Select(course => new
            {
                Course = course,
                Score = CalculateCourseRelevanceScore(course, profile)
            })
            .OrderByDescending(x => x.Score)
            .Take(5)
            .Select(x => x.Course)
            .ToList();

        if (!topCourses.Any()) return null;

        var totalPrice = topCourses.Sum(c => c.Price);
        var discountedPrice = totalPrice * 0.80m; // 20% discount

        // Create and save the learning path
        var learningPath = new LearningPath
        {
            Id = Guid.NewGuid(),
            Title = "Your Personalized Learning Journey",
            Description = $"Based on your assessment, we've curated {topCourses.Count} courses that best match your interests and skill level. This custom path is designed just for you!",
            Price = discountedPrice,
            Status = "published",
            IsCustomPath = true,
            CreatedByUserId = userId,
            SourceAssessmentId = assessmentId,
            CreatedAt = DateTime.UtcNow
        };

        await learningPathRepository.AddAsync(learningPath);

        // Add courses to the path
        for (int i = 0; i < topCourses.Count; i++)
        {
            var pathCourse = new PathCourse
            {
                PathId = learningPath.Id,
                CourseId = topCourses[i].Id,
                OrderIndex = i + 1
            };
            await learningPathRepository.AddPathCourseAsync(pathCourse);
        }

        // Auto-enroll user
        var enrollment = new UserLearningPathEnrollment
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            PathId = learningPath.Id,
            Status = "active",
            ProgressPercentage = 0,
            EnrolledAt = DateTime.UtcNow
        };
        await enrollmentRepository.AddAsync(enrollment);

        await learningPathRepository.SaveChangesAsync();

        return new LearningPathRecommendationDto
        {
            RecommendationTitle = learningPath.Title,
            RecommendationReason = learningPath.Description,
            SkillLevel = char.ToUpper(profile.PrimarySkillLevel[0]) + profile.PrimarySkillLevel[1..],
            RecommendedCourses = topCourses.Select((c, index) => new CourseViewModel
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Price = c.Price,
                ImageUrl = c.ImageUrl,
                InstructorName = c.Instructor?.Username ?? "Unknown",
                CategoryName = c.Category?.Name ?? "General",
                OrderIndex = index + 1
            }).ToList(),
            PathId = learningPath.Id,
            PathPrice = discountedPrice
        };
    }

    private async Task<List<LearningPathRecommendationDto>> FindMatchingPrebuiltPaths(
        UserAssessmentProfile profile)
    {
        var recommendations = new List<LearningPathRecommendationDto>();
        var allPaths = await learningPathRepository.GetPublishedPathsAsync();

        foreach (var path in allPaths.Where(p => !p.IsCustomPath))
        {
            // Check if path matches user's category interests
            var pathCategories = path.PathCourses
                .Select(pc => pc.Course.CategoryId)
                .Distinct()
                .ToList();

            var matchScore = pathCategories
                .Sum(catId => profile.CategoryInterests.GetValueOrDefault(catId, 0));

            if (matchScore > 0)
            {
                var coursesInPath = path.PathCourses
                    .OrderBy(pc => pc.OrderIndex)
                    .Select((pc, index) => new CourseViewModel
                    {
                        Id = pc.Course.Id,
                        Title = pc.Course.Title,
                        Description = pc.Course.Description,
                        Price = pc.Course.Price,
                        ImageUrl = pc.Course.ImageUrl,
                        InstructorName = pc.Course.Instructor?.Username ?? "Unknown",
                        CategoryName = pc.Course.Category?.Name ?? "General",
                        OrderIndex = index + 1
                    })
                    .ToList();

                recommendations.Add(new LearningPathRecommendationDto
                {
                    RecommendationTitle = path.Title,
                    RecommendationReason = $"This pre-built learning path matches your interests! It includes {coursesInPath.Count} carefully structured courses.",
                    SkillLevel = char.ToUpper(profile.PrimarySkillLevel[0]) + profile.PrimarySkillLevel[1..],
                    RecommendedCourses = coursesInPath,
                    PathId = path.Id,
                    PathPrice = path.Price
                });
            }
        }

        return recommendations
            .OrderByDescending(r => r.RecommendedCourses.Count)
            .Take(2)
            .ToList();
    }

    /// <summary>
    /// Internal class to hold analyzed user profile from assessment
    /// </summary>
    private class UserAssessmentProfile
    {
        public Dictionary<Guid, int> CategoryInterests { get; set; } = new();
        public Dictionary<string, int> SkillLevelScores { get; set; } = new();
        public List<string> InterestIndicators { get; set; } = new();
        public List<string> ExperienceIndicators { get; set; } = new();
        public List<string> GoalIndicators { get; set; } = new();
        public string PrimarySkillLevel { get; set; } = "beginner";
    }
}
