using Backend.Services;
using Backend.DTOs.Game;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers;

[ApiController]
[Route("api/game/player")]
[Authorize]
public class PlayerController : ControllerBase
{
    private readonly PlayerStatsService _playerStatsService;

    public PlayerController(PlayerStatsService playerStatsService)
    {
        _playerStatsService = playerStatsService;
    }

    [HttpGet("status")]
    public async Task<ActionResult<PlayerStatusDto>> GetStatus()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var status = await _playerStatsService.GetPlayerStatus(userId);
        return Ok(status);
    }
}
