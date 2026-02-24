using Microsoft.AspNetCore.Mvc;
using Lumina_Learning.Models;
using Lumina_Learning.DTOs;
using Supabase;

namespace Lumina_Learning.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly Supabase.Client _supabase;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(Supabase.Client supabase, ILogger<StudentsController> logger)
    {
        _supabase = supabase;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<StudentDto>>> GetStudents([FromQuery] PaginationParams paginationParams)
    {
        try
        {
            var table = _supabase.From<Student>();
            
            var response = await table
                .Select("*")
                .Range((paginationParams.PageNumber - 1) * paginationParams.PageSize, 
                       paginationParams.PageNumber * paginationParams.PageSize - 1)
                .Order("created_at", Postgrest.Constants.Ordering.Descending)
                .Get();

            var items = response.Models.Select(s => new StudentDto
            {
                Id = s.Id,
                StudentCode = s.StudentCode,
                FirstName = s.FirstName,
                LastName = s.LastName,
                DateOfBirth = s.DateOfBirth,
                Gender = s.Gender,
                Email = s.Email,
                Phone = s.Phone,
                Address = s.Address,
                ParentName = s.ParentName,
                ParentPhone = s.ParentPhone,
                CreatedAt = s.CreatedAt
            }).ToList();

            var countResponse = await table.Select("*").Get();

            var result = new PagedResult<StudentDto>
            {
                Items = items,
                TotalCount = countResponse.Models.Count,
                PageNumber = paginationParams.PageNumber,
                PageSize = paginationParams.PageSize
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching students");
            return StatusCode(500, new { message = "An error occurred while fetching students.", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetStudent(int id)
    {
        try
        {
            var response = await _supabase.From<Student>()
                .Where(s => s.Id == id)
                .Single();

            if (response == null)
            {
                return NotFound(new { message = $"Student with ID {id} not found" });
            }

            var studentDto = new StudentDto
            {
                Id = response.Id,
                StudentCode = response.StudentCode,
                FirstName = response.FirstName,
                LastName = response.LastName,
                DateOfBirth = response.DateOfBirth,
                Gender = response.Gender,
                Email = response.Email,
                Phone = response.Phone,
                Address = response.Address,
                ParentName = response.ParentName,
                ParentPhone = response.ParentPhone,
                CreatedAt = response.CreatedAt
            };

            return Ok(studentDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching student {Id}", id);
            return NotFound(new { message = $"Student with ID {id} not found" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<StudentDto>> PostStudent([FromBody] CreateStudentDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var student = new Student
            {
                StudentCode = createDto.StudentCode,
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                DateOfBirth = createDto.DateOfBirth,
                Gender = createDto.Gender,
                Email = createDto.Email,
                Phone = createDto.Phone,
                Address = createDto.Address,
                ParentName = createDto.ParentName,
                ParentPhone = createDto.ParentPhone,
                CreatedAt = DateTime.UtcNow
            };

            var response = await _supabase.From<Student>()
                .Insert(student);

            var created = response.Models.First();

            var studentDto = new StudentDto
            {
                Id = created.Id,
                StudentCode = created.StudentCode,
                FirstName = created.FirstName,
                LastName = created.LastName,
                DateOfBirth = created.DateOfBirth,
                Gender = created.Gender,
                Email = created.Email,
                Phone = created.Phone,
                Address = created.Address,
                ParentName = created.ParentName,
                ParentPhone = created.ParentPhone,
                CreatedAt = created.CreatedAt
            };

            return CreatedAtAction(nameof(GetStudent), new { id = created.Id }, studentDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating student");
            return StatusCode(500, new { message = "An error occurred while creating the student.", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutStudent(int id, [FromBody] UpdateStudentDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var student = new Student
            {
                Id = id,
                StudentCode = updateDto.StudentCode,
                FirstName = updateDto.FirstName,
                LastName = updateDto.LastName,
                DateOfBirth = updateDto.DateOfBirth,
                Gender = updateDto.Gender,
                Email = updateDto.Email,
                Phone = updateDto.Phone,
                Address = updateDto.Address,
                ParentName = updateDto.ParentName,
                ParentPhone = updateDto.ParentPhone
            };

            await _supabase.From<Student>()
                .Where(s => s.Id == id)
                .Update(student);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating student {Id}", id);
            return NotFound(new { message = $"Student with ID {id} not found" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        try
        {
            await _supabase.From<Student>()
                .Where(s => s.Id == id)
                .Delete();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting student {Id}", id);
            return NotFound(new { message = $"Student with ID {id} not found" });
        }
    }
}
