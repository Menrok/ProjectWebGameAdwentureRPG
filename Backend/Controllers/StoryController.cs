using Backend.Services;
using Backend.DTOs.Game;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers;

[ApiController]
[Route("api/game/story")]
[Authorize]
public class StoryController : ControllerBase
{
    private readonly StoryService _storyService;

    public StoryController(StoryService storyService)
    {
        _storyService = storyService;
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentNode()
    {
        var playerId = GetPlayerId();
        var node = await _storyService.GetCurrentNode(playerId);
        return Ok(node);
    }

    [HttpPost("choose")]
    public async Task<IActionResult> Choose([FromBody] StoryChoiceRequest request)
    {
        var playerId = GetPlayerId();

        var nextNode = await _storyService.Choose(
            playerId,
            request.NextNodeId
        );

        return Ok(nextNode);
    }

    private int GetPlayerId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
