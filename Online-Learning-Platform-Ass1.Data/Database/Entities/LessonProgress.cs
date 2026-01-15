using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Learning_Platform_Ass1.Data.Database.Entities;

[Table("Lesson_Progress")]
public class LessonProgress
{
    [Key]
    [Column("progress_id")]
    public Guid Id { get; set; }

    [Column("enrollment_id")]
    public Guid EnrollmentId { get; set; }

    [ForeignKey(nameof(EnrollmentId))]
    public Enrollment Enrollment { get; set; } = null!;

    [Column("lesson_id")]
    public Guid LessonId { get; set; }

    [ForeignKey(nameof(LessonId))]
    public Lesson Lesson { get; set; } = null!;

    [Column("is_completed")]
    public bool IsCompleted { get; set; }

    [Column("last_watched_position")]
    public int? LastWatchedPosition { get; set; } // in seconds maybe
}
