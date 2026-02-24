using Postgrest.Attributes;
using Postgrest.Models;

namespace Lumina_Learning.Models;

[Table("classroom_students")]
public class ClassroomStudent : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("classroom_id")]
    public int ClassroomId { get; set; }

    [Column("student_id")]
    public int StudentId { get; set; }

    [Column("seat_number")]
    public string? SeatNumber { get; set; }

    [Column("enrollment_date")]
    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

    [Column("status")]
    public string Status { get; set; } = "active"; // active, inactive, transferred

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
