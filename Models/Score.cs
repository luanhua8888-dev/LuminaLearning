using Postgrest.Attributes;
using Postgrest.Models;

namespace Lumina_Learning.Models;

[Table("scores")]
public class Score : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("student_id")]
    public int StudentId { get; set; }

    [Column("subject_id")]
    public int SubjectId { get; set; }

    [Column("classroom_id")]
    public int ClassroomId { get; set; }

    [Column("score_type")]
    public string ScoreType { get; set; } = string.Empty; // oral, 15min, 45min, midterm, final

    [Column("score_value")]
    public decimal ScoreValue { get; set; }

    [Column("coefficient")]
    public int Coefficient { get; set; } = 1; // H? s?

    [Column("semester")]
    public int Semester { get; set; } // 1 or 2

    [Column("academic_year")]
    public string AcademicYear { get; set; } = string.Empty; // e.g., "2024-2025"

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
