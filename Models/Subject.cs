using Postgrest.Attributes;
using Postgrest.Models;

namespace Lumina_Learning.Models;

[Table("subjects")]
public class Subject : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("subject_code")]
    public string SubjectCode { get; set; } = string.Empty;

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }

    [Column("credits")]
    public int Credits { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
