using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Learning_Platform_Ass1.Data.Database.Entities;

[Table("Quizzes")]
public class Quiz
{
    [Key]
    [Column("quiz_id")]
    public Guid Id { get; set; }

    [Column("lesson_id")]
    public Guid LessonId { get; set; }

    [ForeignKey(nameof(LessonId))]
    public Lesson Lesson { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    [Column("title")]
    public string Title { get; set; } = null!;

    [Column("passing_score")]
    public double PassingScore { get; set; } // e.g. 70.0 for 70%

    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
}
