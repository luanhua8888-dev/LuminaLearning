using System.ComponentModel.DataAnnotations;

namespace Lumina_Learning.DTOs;

public class EnrollStudentDto
{
    [Required(ErrorMessage = "ID h?c sinh là b?t bu?c")]
    public int StudentId { get; set; }

    [StringLength(10, ErrorMessage = "S? ch? ng?i không ???c v??t quá 10 ký t?")]
    public string? SeatNumber { get; set; }
}

public class UpdateSeatDto
{
    [Required(ErrorMessage = "S? ch? ng?i là b?t bu?c")]
    [StringLength(10, ErrorMessage = "S? ch? ng?i không ???c v??t quá 10 ký t?")]
    public required string SeatNumber { get; set; }
}

public class ClassroomStudentDto
{
    public int Id { get; set; }
    public int ClassroomId { get; set; }
    public int StudentId { get; set; }
    public string? SeatNumber { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public required string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ClassroomWithStudentsDto
{
    public required ClassroomDto Classroom { get; set; }
    public List<StudentDto> Students { get; set; } = new();
    public int TotalStudents { get; set; }
}
