using System.ComponentModel.DataAnnotations;

namespace Lumina_Learning.DTOs;

public class CreateSubjectDto
{
    [Required(ErrorMessage = "Mã môn h?c là b?t bu?c")]
    [StringLength(20, ErrorMessage = "Mã môn h?c không ???c v??t quá 20 ký t?")]
    public required string SubjectCode { get; set; }

    [Required(ErrorMessage = "Tên môn h?c là b?t bu?c")]
    [StringLength(100, ErrorMessage = "Tên môn h?c không ???c v??t quá 100 ký t?")]
    public required string Name { get; set; }

    [StringLength(500, ErrorMessage = "Mô t? không ???c v??t quá 500 ký t?")]
    public string? Description { get; set; }

    [Range(1, 10, ErrorMessage = "S? tín ch? ph?i t? 1 ??n 10")]
    public int Credits { get; set; } = 1;
}

public class UpdateSubjectDto
{
    [Required(ErrorMessage = "Mã môn h?c là b?t bu?c")]
    [StringLength(20, ErrorMessage = "Mã môn h?c không ???c v??t quá 20 ký t?")]
    public required string SubjectCode { get; set; }

    [Required(ErrorMessage = "Tên môn h?c là b?t bu?c")]
    [StringLength(100, ErrorMessage = "Tên môn h?c không ???c v??t quá 100 ký t?")]
    public required string Name { get; set; }

    [StringLength(500, ErrorMessage = "Mô t? không ???c v??t quá 500 ký t?")]
    public string? Description { get; set; }

    [Range(1, 10, ErrorMessage = "S? tín ch? ph?i t? 1 ??n 10")]
    public int Credits { get; set; } = 1;
}

public class SubjectDto
{
    public int Id { get; set; }
    public required string SubjectCode { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int Credits { get; set; }
    public DateTime CreatedAt { get; set; }
}
