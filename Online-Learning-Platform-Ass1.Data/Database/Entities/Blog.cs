using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Learning_Platform_Ass1.Data.Database.Entities;

[Table("Blogs")]
public class Blog
{
    [Key]
    [Column("blog_id")]
    public Guid Id { get; set; }

    [Column("author_id")]
    public Guid AuthorId { get; set; }

    [ForeignKey(nameof(AuthorId))]
    public User Author { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    [Column("title")]
    public string Title { get; set; } = null!;

    [Required]
    [Column("content")]
    public string Content { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    [Column("status")]
    public string Status { get; set; } = "draft";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
