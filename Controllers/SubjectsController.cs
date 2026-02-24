using Microsoft.AspNetCore.Mvc;
using Lumina_Learning.Models;
using Lumina_Learning.DTOs;
using Supabase;

namespace Lumina_Learning.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectsController : ControllerBase
{
    private readonly Supabase.Client _supabase;
    private readonly ILogger<SubjectsController> _logger;

    public SubjectsController(Supabase.Client supabase, ILogger<SubjectsController> logger)
    {
        _supabase = supabase;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<SubjectDto>>> GetSubjects([FromQuery] PaginationParams paginationParams)
    {
        try
        {
            var table = _supabase.From<Subject>();
            
            var response = await table
                .Select("*")
                .Range((paginationParams.PageNumber - 1) * paginationParams.PageSize, 
                       paginationParams.PageNumber * paginationParams.PageSize - 1)
                .Order("created_at", Postgrest.Constants.Ordering.Descending)
                .Get();

            var items = response.Models.Select(s => new SubjectDto
            {
                Id = s.Id,
                SubjectCode = s.SubjectCode,
                Name = s.Name,
                Description = s.Description,
                Credits = s.Credits,
                CreatedAt = s.CreatedAt
            }).ToList();

            var countResponse = await table.Select("*").Get();

            var result = new PagedResult<SubjectDto>
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
            _logger.LogError(ex, "Error fetching subjects");
            return StatusCode(500, new { message = "An error occurred while fetching subjects.", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SubjectDto>> GetSubject(int id)
    {
        try
        {
            var response = await _supabase.From<Subject>()
                .Where(s => s.Id == id)
                .Single();

            if (response == null)
            {
                return NotFound(new { message = $"Subject with ID {id} not found" });
            }

            var subjectDto = new SubjectDto
            {
                Id = response.Id,
                SubjectCode = response.SubjectCode,
                Name = response.Name,
                Description = response.Description,
                Credits = response.Credits,
                CreatedAt = response.CreatedAt
            };

            return Ok(subjectDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching subject {Id}", id);
            return NotFound(new { message = $"Subject with ID {id} not found" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<SubjectDto>> PostSubject([FromBody] CreateSubjectDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var subject = new Subject
            {
                SubjectCode = createDto.SubjectCode,
                Name = createDto.Name,
                Description = createDto.Description,
                Credits = createDto.Credits,
                CreatedAt = DateTime.UtcNow
            };

            var response = await _supabase.From<Subject>()
                .Insert(subject);

            var created = response.Models.First();

            var subjectDto = new SubjectDto
            {
                Id = created.Id,
                SubjectCode = created.SubjectCode,
                Name = created.Name,
                Description = created.Description,
                Credits = created.Credits,
                CreatedAt = created.CreatedAt
            };

            return CreatedAtAction(nameof(GetSubject), new { id = created.Id }, subjectDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating subject");
            return StatusCode(500, new { message = "An error occurred while creating the subject.", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutSubject(int id, [FromBody] UpdateSubjectDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var subject = new Subject
            {
                Id = id,
                SubjectCode = updateDto.SubjectCode,
                Name = updateDto.Name,
                Description = updateDto.Description,
                Credits = updateDto.Credits
            };

            await _supabase.From<Subject>()
                .Where(s => s.Id == id)
                .Update(subject);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating subject {Id}", id);
            return NotFound(new { message = $"Subject with ID {id} not found" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubject(int id)
    {
        try
        {
            await _supabase.From<Subject>()
                .Where(s => s.Id == id)
                .Delete();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting subject {Id}", id);
            return NotFound(new { message = $"Subject with ID {id} not found" });
        }
    }
}
