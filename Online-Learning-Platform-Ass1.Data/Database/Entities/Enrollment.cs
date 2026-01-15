using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Learning_Platform_Ass1.Data.Database.Entities;

[Table("Enrollments")]
public class Enrollment
{
    [Key]
    [Column("enrollment_id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [Column("course_id")]
    public Guid CourseId { get; set; }

    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; } = null!;

    [Column("enrolled_at")]
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(50)]
    [Column("status")]
    public string Status { get; set; } = "active"; // active, completed, dropped

    // One-to-Many with LessonProgress
    public ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
    
    // One-to-Many (or One-to-One?) with Certificate. ERD: Certificate has enrollment_id FK unique? Usually 1 enrollment -> 0/1 Certificate
    // ERD shows ERmandOne (Cert) -> ERoneToMany (Enrollment) on enrollment_id link? arrow direction seems 1 cert has 1 enrollment. 
    // Usually one enrollment has one certificate.
    public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
}
