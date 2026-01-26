using Backend.Data;
using Backend.Models.Game;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend.Controllers;

[ApiController]
[Route("api/game")]
[Authorize]
public class GameController : ControllerBase
{
    private readonly GameDbContext _db;
    private readonly LocationService _locationService;
    private readonly LocationActionService _locationActionService;

    public GameController(
        GameDbContext db,
        LocationService locationService,
        LocationActionService locationActionService)
    {
        _db = db;
        _locationService = locationService;
        _locationActionService = locationActionService;
    }

    [HttpGet("state")]
    public async Task<IActionResult> GetGameState()
    {
        var player = await GetPlayer();

        if (!string.IsNullOrEmpty(player.CurrentStoryNode))
        {
            return Ok(new
            {
                Mode = "Story",
                StoryNode = player.CurrentStoryNode
            });
        }

        var location = _locationService.GetCurrentLocation(player);
        var connected = _locationService.GetConnectedLocations(player);
        var actions = _locationActionService.GetAvailableActions(player);

        return Ok(new
        {
            Mode = "World",
            Player = new
            {
                player.Id,
                player.Name,
                player.Health,
                player.MaxHealth
            },
            Location = new
            {
                location.Id,
                location.Name,
                location.Description
            },
            ConnectedLocations = connected.Select(l => new
            {
                l.Id,
                l.Name
            }),
            Actions = actions
        });
    }

    [HttpPost("location/move/{locationId}")]
    public async Task<IActionResult> Move(string locationId)
    {
        var player = await GetPlayer();

        _locationService.MovePlayer(player, locationId);

        await _db.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("location/action/{actionId}")]
    public async Task<IActionResult> DoAction(string actionId)
    {
        var player = await GetPlayer();

        var resultText = _locationActionService.Execute(player, actionId);

        await _db.SaveChangesAsync();

        return Ok(new
        {
            Result = resultText
        });
    }

    private async Task<Player> GetPlayer()
    {
        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var player = await _db.Players
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (player == null)
            throw new Exception("Player not found");

        return player;
    }
}
