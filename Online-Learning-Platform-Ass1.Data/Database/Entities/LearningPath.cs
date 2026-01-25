using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Learning_Platform_Ass1.Data.Database.Entities;

[Table("Learning_Paths")]
public class LearningPath
{
    [Key]
    [Column("path_id")]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    [Column("title")]
    public string Title { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("price", TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("status")]
    public string Status { get; set; } = "draft";

    /// <summary>
    /// If set, this is a custom/personalized path created for a specific user from assessment
    /// </summary>
    [Column("created_by_user_id")]
    public Guid? CreatedByUserId { get; set; }

    [ForeignKey(nameof(CreatedByUserId))]
    public User? CreatedByUser { get; set; }

    /// <summary>
    /// The assessment that generated this custom path (if applicable)
    /// </summary>
    [Column("source_assessment_id")]
    public Guid? SourceAssessmentId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// True if this is a custom path generated from assessment
    /// </summary>
    [Column("is_custom_path")]
    public bool IsCustomPath { get; set; } = false;

    // Many-to-Many with Course via PathCourses
    public ICollection<PathCourse> PathCourses { get; set; } = new List<PathCourse>();

    // One-to-Many with UserLearningPathEnrollment
    public ICollection<UserLearningPathEnrollment> UserEnrollments { get; set; } = new List<UserLearningPathEnrollment>();
}
