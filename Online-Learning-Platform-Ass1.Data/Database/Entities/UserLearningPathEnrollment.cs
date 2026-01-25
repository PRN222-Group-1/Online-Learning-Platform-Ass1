using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Learning_Platform_Ass1.Data.Database.Entities;

[Table("User_Learning_Path_Enrollments")]
public class UserLearningPathEnrollment
{
    [Key]
    [Column("enrollment_id")]
    public Guid Id { get; set; }

    [Required]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [Required]
    [Column("path_id")]
    public Guid PathId { get; set; }

    [ForeignKey(nameof(PathId))]
    public LearningPath LearningPath { get; set; } = null!;

    [Required]
    [Column("enrolled_at")]
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

    [Column("completed_at")]
    public DateTime? CompletedAt { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("status")]
    public string Status { get; set; } = "active"; // active, in_progress, completed, dropped

    // Track overall progress
    [Column("progress_percentage")]
    public int ProgressPercentage { get; set; } = 0;
}
