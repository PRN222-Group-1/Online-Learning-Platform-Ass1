using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Learning_Platform_Ass1.Data.Database.Entities;

[Table("Quiz_Attempts")]
public class QuizAttempt
{
    [Key]
    [Column("attempt_id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [Column("quiz_id")]
    public Guid QuizId { get; set; }

    [ForeignKey(nameof(QuizId))]
    public Quiz Quiz { get; set; } = null!;

    [Column("score")]
    public double Score { get; set; }

    [Column("passed")]
    public bool Passed { get; set; }

    [Column("attempted_at")]
    public DateTime AttemptedAt { get; set; } = DateTime.UtcNow;
}
