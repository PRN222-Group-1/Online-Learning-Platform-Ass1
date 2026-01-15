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

    // Many-to-Many with Course via PathCourses
    public ICollection<PathCourse> PathCourses { get; set; } = new List<PathCourse>();
}
