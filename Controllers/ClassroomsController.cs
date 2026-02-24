using Microsoft.AspNetCore.Mvc;
using Lumina_Learning.Models;
using Lumina_Learning.DTOs;
using Supabase;

namespace Lumina_Learning.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassroomsController : ControllerBase
{
    private readonly Supabase.Client _supabase;
    private readonly ILogger<ClassroomsController> _logger;

    public ClassroomsController(Supabase.Client supabase, ILogger<ClassroomsController> logger)
    {
        _supabase = supabase;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ClassroomDto>>> GetClassrooms([FromQuery] PaginationParams paginationParams)
    {
        try
        {
            var table = _supabase.From<Classroom>();
            
            var response = await table
                .Select("*")
                .Range((paginationParams.PageNumber - 1) * paginationParams.PageSize, 
                       paginationParams.PageNumber * paginationParams.PageSize - 1)
                .Order("created_at", Postgrest.Constants.Ordering.Descending)
                .Get();

            var items = response.Models.Select(c => new ClassroomDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedAt = c.CreatedAt
            }).ToList();

            var countResponse = await table.Select("*").Get();

            var result = new PagedResult<ClassroomDto>
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
            _logger.LogError(ex, "Error fetching classrooms");
            return StatusCode(500, new { message = "An error occurred while fetching classrooms.", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClassroomDto>> GetClassroom(int id)
    {
        try
        {
            var response = await _supabase.From<Classroom>()
                .Where(c => c.Id == id)
                .Single();

            if (response == null)
            {
                return NotFound(new { message = $"Classroom with ID {id} not found" });
            }

            var classroomDto = new ClassroomDto
            {
                Id = response.Id,
                Name = response.Name,
                Description = response.Description,
                CreatedAt = response.CreatedAt
            };

            return Ok(classroomDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching classroom {Id}", id);
            return NotFound(new { message = $"Classroom with ID {id} not found" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ClassroomDto>> PostClassroom([FromBody] CreateClassroomDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var classroom = new Classroom
            {
                Name = createDto.Name,
                Description = createDto.Description,
                CreatedAt = DateTime.UtcNow
            };

            var response = await _supabase.From<Classroom>()
                .Insert(classroom);

            var created = response.Models.First();

            var classroomDto = new ClassroomDto
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description,
                CreatedAt = created.CreatedAt
            };

            return CreatedAtAction(nameof(GetClassroom), new { id = created.Id }, classroomDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating classroom");
            return StatusCode(500, new { message = "An error occurred while creating the classroom.", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutClassroom(int id, [FromBody] UpdateClassroomDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var classroom = new Classroom
            {
                Id = id,
                Name = updateDto.Name,
                Description = updateDto.Description
            };

            await _supabase.From<Classroom>()
                .Where(c => c.Id == id)
                .Update(classroom);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating classroom {Id}", id);
            return NotFound(new { message = $"Classroom with ID {id} not found" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClassroom(int id)
    {
        try
        {
            await _supabase.From<Classroom>()
                .Where(c => c.Id == id)
                .Delete();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting classroom {Id}", id);
            return NotFound(new { message = $"Classroom with ID {id} not found" });
        }
    }
}
