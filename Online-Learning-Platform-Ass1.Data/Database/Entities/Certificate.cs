using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Learning_Platform_Ass1.Data.Database.Entities;

[Table("Certificates")]
public class Certificate
{
    [Key]
    [Column("certificate_id")]
    public Guid Id { get; set; }

    [Column("enrollment_id")]
    public Guid EnrollmentId { get; set; }

    [ForeignKey(nameof(EnrollmentId))]
    public Enrollment Enrollment { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [Column("serial_number")]
    public string SerialNumber { get; set; } = null!;

    [Column("issue_date")]
    public DateTime IssueDate { get; set; } = DateTime.UtcNow;

    [Column("pdf_url")]
    public string? PdfUrl { get; set; }
}
