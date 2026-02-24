using System.ComponentModel.DataAnnotations;

namespace Lumina_Learning.DTOs;

public class CreateClassroomDto
{
    [Required(ErrorMessage = "Tên l?p h?c là b?t bu?c")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên l?p h?c ph?i t? 3 ??n 100 ký t?")]
    public required string Name { get; set; }

    [StringLength(500, ErrorMessage = "Mô t? không ???c v??t quá 500 ký t?")]
    public string? Description { get; set; }
}

public class UpdateClassroomDto
{
    [Required(ErrorMessage = "Tên l?p h?c là b?t bu?c")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên l?p h?c ph?i t? 3 ??n 100 ký t?")]
    public required string Name { get; set; }

    [StringLength(500, ErrorMessage = "Mô t? không ???c v??t quá 500 ký t?")]
    public string? Description { get; set; }
}

public class ClassroomDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
