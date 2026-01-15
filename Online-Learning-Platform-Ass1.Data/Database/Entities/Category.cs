using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Learning_Platform_Ass1.Data.Database.Entities;

[Table("Categories")]
public class Category
{
    [Key]
    [Column("category_id")]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("parent_id")]
    public Guid? ParentId { get; set; }

    [ForeignKey(nameof(ParentId))]
    public Category? ParentCategory { get; set; }

    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
    
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
