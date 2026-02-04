using Backend.Data;
using Backend.Models.Game;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Backend.DTOs.Game;

namespace Backend.Controllers;

[ApiController]
[Route("api/game")]
[Authorize]
public class GameController : ControllerBase
{
    private readonly GameDbContext _db;
    private readonly LocationService _locationService;
    private readonly LocationActionService _locationActionService;

    public GameController(GameDbContext db, LocationService locationService, LocationActionService locationActionService)
    {
        _db = db;
        _locationService = locationService;
        _locationActionService = locationActionService;
    }

    [HttpGet("state")]
    public async Task<IActionResult> GetGameState()
    {
        var player = await GetPlayer();

        if (!string.IsNullOrEmpty(player.CurrentStoryNodeId))
        {
            return Ok(new
            {
                mode = "Story",
                storyNode = player.CurrentStoryNodeId
            });
        }

        var location = _locationService.GetCurrentLocation(player);
        var connected = _locationService.GetConnectedLocations(player);
        var actions = _locationActionService.GetAvailableActions(player);

        return Ok(new
        {
            mode = "World",
            player = new
            {
                player.Id,
                player.Name,
                player.Health,
                player.MaxHealth
            },
            location = new
            {
                location.Id,
                location.Name,
                location.Description
            },
            connectedLocations = connected.Select(l => new
            {
                l.Id,
                l.Name
            }),
            actions,
            flags = player.Flags.Select(f => new
            {
                flag = f.Flag
            })
        });
    }

    [HttpPost("location/move/{locationId}")]
    public async Task<IActionResult> Move(string locationId)
    {
        var player = await GetPlayer();

        _locationService.MovePlayer(player, locationId);

        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }

    [HttpPost("location/action/{actionId}")]
    public async Task<ActionResult<ActionResultDto>> DoAction(string actionId)
    {
        var player = await GetPlayer();

        var result = _locationActionService.Execute(player, actionId);

        await _db.SaveChangesAsync();
        return Ok(result);
    }

    private async Task<Player> GetPlayer()
    {
        var userId = GetUserId();

        var player = await _db.Players.Include(p => p.Flags).FirstOrDefaultAsync(p => p.UserId == userId);

        if (player == null)
            throw new Exception("Player not found");

        return player;
    }

    protected int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
