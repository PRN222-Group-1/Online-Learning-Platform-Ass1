using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Learning_Platform_Ass1.Data.Database.Entities;

[Table("Options")]
public class Option
{
    [Key]
    [Column("option_id")]
    public Guid Id { get; set; }

    [Column("question_id")]
    public Guid QuestionId { get; set; }

    [ForeignKey(nameof(QuestionId))]
    public Question Question { get; set; } = null!;

    [Required]
    [Column("text")]
    public string Text { get; set; } = null!;

    [Column("is_correct")]
    public bool IsCorrect { get; set; }
}
