using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Learning_Platform_Ass1.Data.Database.Entities;

[Table("Lessons")]
public class Lesson
{
    [Key]
    [Column("lesson_id")]
    public Guid Id { get; set; }

    [Column("module_id")]
    public Guid ModuleId { get; set; }

    [ForeignKey(nameof(ModuleId))]
    public Module Module { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    [Column("title")]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    [Column("type")]
    public string Type { get; set; } = "video"; // video, text, quiz

    [Column("content_url")]
    public string? ContentUrl { get; set; } // For video URL or Text content blob/link

    [Column("duration")]
    public int? Duration { get; set; } // In seconds/minutes

    [Column("order_index")]
    public int OrderIndex { get; set; }

    public ICollection<LessonProgress> Progresses { get; set; } = new List<LessonProgress>();
    // One-to-One or One-to-Many relation with Quiz? ERD usually shows Lesson 1-N Quiz or Quiz linked to Lesson.
    // Based on ERD: Quizzes table has lesson_id FK.
    public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
}
