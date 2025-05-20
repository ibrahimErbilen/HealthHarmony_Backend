using HealthHarmony.Entities.DTOs.Coach;
using HealthHarmony.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CoachController : ControllerBase
{
    private readonly ICoachService _coachService;

    public CoachController(ICoachService coachService)
    {
        _coachService = coachService;
    }

    [HttpPost("add")]
    public IActionResult AddCoach([FromBody] CoachCreateDTO coachDto)
    {
        _coachService.AddCoach(coachDto);
        return Ok("Coach başarıyla eklendi.");
    }

    [HttpPut("update")]
    public IActionResult UpdateCoach([FromBody] CoachDTO coachDto)
    {
        _coachService.UpdateCoach(coachDto);
        return Ok("Coach başarıyla güncellendi.");
    }

    [HttpDelete("delete/{id}")]
    public IActionResult DeleteCoach(Guid id)
    {
        _coachService.DeleteCoach(id);
        return Ok("Coach başarıyla silindi.");
    }

    [HttpGet("search")]
    public IActionResult Search(string name)
    {
        var coaches = _coachService.SearchCoachByName(name);
        return Ok(coaches);
    }

    [HttpGet("get-by-invitation-code/{code}")]
    public IActionResult GetByInvitationCode(string code)
    {
        var coach = _coachService.GetCoachByInvitationCode(code);
        if (coach == null)
            return NotFound("Coach bulunamadı.");

        return Ok(coach);
    }

}
