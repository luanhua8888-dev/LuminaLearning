using System.ComponentModel.DataAnnotations;

namespace Lumina_Learning.DTOs;

public class CreateStudentDto
{
    [Required(ErrorMessage = "Mã h?c sinh là b?t bu?c")]
    [StringLength(20, ErrorMessage = "Mã h?c sinh không ???c v??t quá 20 kı t?")]
    public required string StudentCode { get; set; }

    [Required(ErrorMessage = "Tên là b?t bu?c")]
    [StringLength(50, ErrorMessage = "Tên không ???c v??t quá 50 kı t?")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "H? là b?t bu?c")]
    [StringLength(50, ErrorMessage = "H? không ???c v??t quá 50 kı t?")]
    public required string LastName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [StringLength(10, ErrorMessage = "Gi?i tính không ???c v??t quá 10 kı t?")]
    public string? Gender { get; set; }

    [EmailAddress(ErrorMessage = "Email không h?p l?")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "S? ?i?n tho?i không h?p l?")]
    public string? Phone { get; set; }

    [StringLength(200, ErrorMessage = "??a ch? không ???c v??t quá 200 kı t?")]
    public string? Address { get; set; }

    [StringLength(100, ErrorMessage = "Tên ph? huynh không ???c v??t quá 100 kı t?")]
    public string? ParentName { get; set; }

    [Phone(ErrorMessage = "S? ?i?n tho?i ph? huynh không h?p l?")]
    public string? ParentPhone { get; set; }
}

public class UpdateStudentDto
{
    [Required(ErrorMessage = "Mã h?c sinh là b?t bu?c")]
    [StringLength(20, ErrorMessage = "Mã h?c sinh không ???c v??t quá 20 kı t?")]
    public required string StudentCode { get; set; }

    [Required(ErrorMessage = "Tên là b?t bu?c")]
    [StringLength(50, ErrorMessage = "Tên không ???c v??t quá 50 kı t?")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "H? là b?t bu?c")]
    [StringLength(50, ErrorMessage = "H? không ???c v??t quá 50 kı t?")]
    public required string LastName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [StringLength(10, ErrorMessage = "Gi?i tính không ???c v??t quá 10 kı t?")]
    public string? Gender { get; set; }

    [EmailAddress(ErrorMessage = "Email không h?p l?")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "S? ?i?n tho?i không h?p l?")]
    public string? Phone { get; set; }

    [StringLength(200, ErrorMessage = "??a ch? không ???c v??t quá 200 kı t?")]
    public string? Address { get; set; }

    [StringLength(100, ErrorMessage = "Tên ph? huynh không ???c v??t quá 100 kı t?")]
    public string? ParentName { get; set; }

    [Phone(ErrorMessage = "S? ?i?n tho?i ph? huynh không h?p l?")]
    public string? ParentPhone { get; set; }
}

public class StudentDto
{
    public int Id { get; set; }
    public required string StudentCode { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string FullName => $"{LastName} {FirstName}";
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? ParentName { get; set; }
    public string? ParentPhone { get; set; }
    public DateTime CreatedAt { get; set; }
}
