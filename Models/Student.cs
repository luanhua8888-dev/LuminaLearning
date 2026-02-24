using Postgrest.Attributes;
using Postgrest.Models;

namespace Lumina_Learning.Models;

[Table("students")]
public class Student : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("student_code")]
    public string StudentCode { get; set; } = string.Empty;

    [Column("first_name")]
    public string FirstName { get; set; } = string.Empty;

    [Column("last_name")]
    public string LastName { get; set; } = string.Empty;

    [Column("date_of_birth")]
    public DateTime? DateOfBirth { get; set; }

    [Column("gender")]
    public string? Gender { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("phone")]
    public string? Phone { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Column("parent_name")]
    public string? ParentName { get; set; }

    [Column("parent_phone")]
    public string? ParentPhone { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
