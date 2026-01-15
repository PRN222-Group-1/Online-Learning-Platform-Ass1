using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Learning_Platform_Ass1.Data.Database.Entities;

[Table("Path_Courses")]
public class PathCourse
{
    [Column("path_id")]
    public Guid PathId { get; set; }

    [ForeignKey(nameof(PathId))]
    public LearningPath LearningPath { get; set; } = null!;

    [Column("course_id")]
    public Guid CourseId { get; set; }

    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; } = null!;

    [Column("order_index")]
    public int OrderIndex { get; set; }
}
