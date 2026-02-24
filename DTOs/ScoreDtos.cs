using System.ComponentModel.DataAnnotations;

namespace Lumina_Learning.DTOs;

public class CreateScoreDto
{
    [Required(ErrorMessage = "ID h?c sinh là b?t bu?c")]
    public int StudentId { get; set; }

    [Required(ErrorMessage = "ID môn h?c là b?t bu?c")]
    public int SubjectId { get; set; }

    [Required(ErrorMessage = "ID l?p h?c là b?t bu?c")]
    public int ClassroomId { get; set; }

    [Required(ErrorMessage = "Lo?i ?i?m là b?t bu?c")]
    [StringLength(20, ErrorMessage = "Lo?i ?i?m không ???c v??t quá 20 ký t?")]
    public required string ScoreType { get; set; } // oral, 15min, 45min, midterm, final

    [Required(ErrorMessage = "?i?m s? là b?t bu?c")]
    [Range(0, 10, ErrorMessage = "?i?m s? ph?i t? 0 ??n 10")]
    public decimal ScoreValue { get; set; }

    [Range(1, 3, ErrorMessage = "H? s? ph?i t? 1 ??n 3")]
    public int Coefficient { get; set; } = 1;

    [Required(ErrorMessage = "H?c k? là b?t bu?c")]
    [Range(1, 2, ErrorMessage = "H?c k? ph?i là 1 ho?c 2")]
    public int Semester { get; set; }

    [Required(ErrorMessage = "N?m h?c là b?t bu?c")]
    [StringLength(10, ErrorMessage = "N?m h?c không ???c v??t quá 10 ký t?")]
    public required string AcademicYear { get; set; }

    [StringLength(500, ErrorMessage = "Ghi chú không ???c v??t quá 500 ký t?")]
    public string? Notes { get; set; }
}

public class UpdateScoreDto
{
    [Required(ErrorMessage = "Lo?i ?i?m là b?t bu?c")]
    [StringLength(20, ErrorMessage = "Lo?i ?i?m không ???c v??t quá 20 ký t?")]
    public required string ScoreType { get; set; }

    [Required(ErrorMessage = "?i?m s? là b?t bu?c")]
    [Range(0, 10, ErrorMessage = "?i?m s? ph?i t? 0 ??n 10")]
    public decimal ScoreValue { get; set; }

    [Range(1, 3, ErrorMessage = "H? s? ph?i t? 1 ??n 3")]
    public int Coefficient { get; set; } = 1;

    [Required(ErrorMessage = "H?c k? là b?t bu?c")]
    [Range(1, 2, ErrorMessage = "H?c k? ph?i là 1 ho?c 2")]
    public int Semester { get; set; }

    [Required(ErrorMessage = "N?m h?c là b?t bu?c")]
    [StringLength(10, ErrorMessage = "N?m h?c không ???c v??t quá 10 ký t?")]
    public required string AcademicYear { get; set; }

    [StringLength(500, ErrorMessage = "Ghi chú không ???c v??t quá 500 ký t?")]
    public string? Notes { get; set; }
}

public class ScoreDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    public int ClassroomId { get; set; }
    public required string ScoreType { get; set; }
    public decimal ScoreValue { get; set; }
    public int Coefficient { get; set; }
    public int Semester { get; set; }
    public required string AcademicYear { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class StudentScoreReportDto
{
    public required StudentDto Student { get; set; }
    public required SubjectDto Subject { get; set; }
    public List<ScoreDto> Scores { get; set; } = new();
    public decimal AverageScore { get; set; }
}
