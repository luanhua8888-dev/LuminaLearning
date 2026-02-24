using Microsoft.AspNetCore.Mvc;
using Lumina_Learning.Models;
using Lumina_Learning.DTOs;
using Supabase;

namespace Lumina_Learning.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScoresController : ControllerBase
{
    private readonly Supabase.Client _supabase;
    private readonly ILogger<ScoresController> _logger;

    public ScoresController(Supabase.Client supabase, ILogger<ScoresController> logger)
    {
        _supabase = supabase;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ScoreDto>>> GetScores([FromQuery] PaginationParams paginationParams)
    {
        try
        {
            var table = _supabase.From<Score>();
            
            var response = await table
                .Select("*")
                .Range((paginationParams.PageNumber - 1) * paginationParams.PageSize, 
                       paginationParams.PageNumber * paginationParams.PageSize - 1)
                .Order("created_at", Postgrest.Constants.Ordering.Descending)
                .Get();

            var items = response.Models.Select(s => new ScoreDto
            {
                Id = s.Id,
                StudentId = s.StudentId,
                SubjectId = s.SubjectId,
                ClassroomId = s.ClassroomId,
                ScoreType = s.ScoreType,
                ScoreValue = s.ScoreValue,
                Coefficient = s.Coefficient,
                Semester = s.Semester,
                AcademicYear = s.AcademicYear,
                Notes = s.Notes,
                CreatedAt = s.CreatedAt
            }).ToList();

            var countResponse = await table.Select("*").Get();

            var result = new PagedResult<ScoreDto>
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
            _logger.LogError(ex, "Error fetching scores");
            return StatusCode(500, new { message = "An error occurred while fetching scores.", error = ex.Message });
        }
    }

    [HttpGet("student/{studentId}")]
    public async Task<ActionResult<List<ScoreDto>>> GetStudentScores(int studentId)
    {
        try
        {
            var response = await _supabase.From<Score>()
                .Where(s => s.StudentId == studentId)
                .Order("created_at", Postgrest.Constants.Ordering.Descending)
                .Get();

            var scores = response.Models.Select(s => new ScoreDto
            {
                Id = s.Id,
                StudentId = s.StudentId,
                SubjectId = s.SubjectId,
                ClassroomId = s.ClassroomId,
                ScoreType = s.ScoreType,
                ScoreValue = s.ScoreValue,
                Coefficient = s.Coefficient,
                Semester = s.Semester,
                AcademicYear = s.AcademicYear,
                Notes = s.Notes,
                CreatedAt = s.CreatedAt
            }).ToList();

            return Ok(scores);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching scores for student {StudentId}", studentId);
            return StatusCode(500, new { message = "An error occurred while fetching student scores.", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ScoreDto>> GetScore(int id)
    {
        try
        {
            var response = await _supabase.From<Score>()
                .Where(s => s.Id == id)
                .Single();

            if (response == null)
            {
                return NotFound(new { message = $"Score with ID {id} not found" });
            }

            var scoreDto = new ScoreDto
            {
                Id = response.Id,
                StudentId = response.StudentId,
                SubjectId = response.SubjectId,
                ClassroomId = response.ClassroomId,
                ScoreType = response.ScoreType,
                ScoreValue = response.ScoreValue,
                Coefficient = response.Coefficient,
                Semester = response.Semester,
                AcademicYear = response.AcademicYear,
                Notes = response.Notes,
                CreatedAt = response.CreatedAt
            };

            return Ok(scoreDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching score {Id}", id);
            return NotFound(new { message = $"Score with ID {id} not found" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ScoreDto>> PostScore([FromBody] CreateScoreDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var score = new Score
            {
                StudentId = createDto.StudentId,
                SubjectId = createDto.SubjectId,
                ClassroomId = createDto.ClassroomId,
                ScoreType = createDto.ScoreType,
                ScoreValue = createDto.ScoreValue,
                Coefficient = createDto.Coefficient,
                Semester = createDto.Semester,
                AcademicYear = createDto.AcademicYear,
                Notes = createDto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            var response = await _supabase.From<Score>()
                .Insert(score);

            var created = response.Models.First();

            var scoreDto = new ScoreDto
            {
                Id = created.Id,
                StudentId = created.StudentId,
                SubjectId = created.SubjectId,
                ClassroomId = created.ClassroomId,
                ScoreType = created.ScoreType,
                ScoreValue = created.ScoreValue,
                Coefficient = created.Coefficient,
                Semester = created.Semester,
                AcademicYear = created.AcademicYear,
                Notes = created.Notes,
                CreatedAt = created.CreatedAt
            };

            return CreatedAtAction(nameof(GetScore), new { id = created.Id }, scoreDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating score");
            return StatusCode(500, new { message = "An error occurred while creating the score.", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutScore(int id, [FromBody] UpdateScoreDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var score = new Score
            {
                Id = id,
                ScoreType = updateDto.ScoreType,
                ScoreValue = updateDto.ScoreValue,
                Coefficient = updateDto.Coefficient,
                Semester = updateDto.Semester,
                AcademicYear = updateDto.AcademicYear,
                Notes = updateDto.Notes
            };

            await _supabase.From<Score>()
                .Where(s => s.Id == id)
                .Update(score);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating score {Id}", id);
            return NotFound(new { message = $"Score with ID {id} not found" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteScore(int id)
    {
        try
        {
            await _supabase.From<Score>()
                .Where(s => s.Id == id)
                .Delete();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting score {Id}", id);
            return NotFound(new { message = $"Score with ID {id} not found" });
        }
    }
}
