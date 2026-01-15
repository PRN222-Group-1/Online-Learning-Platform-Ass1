using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Learning_Platform_Ass1.Data.Database.Entities;

[Table("Questions")]
public class Question
{
    [Key]
    [Column("question_id")]
    public Guid Id { get; set; }

    [Column("quiz_id")]
    public Guid QuizId { get; set; }

    [ForeignKey(nameof(QuizId))]
    public Quiz Quiz { get; set; } = null!;

    [Required]
    [Column("content")]
    public string Content { get; set; } = null!; // The question text

    [Required]
    [MaxLength(20)]
    [Column("type")]
    public string Type { get; set; } = "single_choice"; // single_choice, multiple_choice

    public ICollection<Option> Options { get; set; } = new List<Option>();
}
