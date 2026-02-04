using Backend.Data;
using Backend.DTOs.Game;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend.Controllers;

[ApiController]
[Route("api/game/quests")]
[Authorize]
public class QuestController : ControllerBase
{
    private readonly GameDbContext _context;

    public QuestController(GameDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetQuests()
    {
        var playerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var player = await _context.Players
            .Include(p => p.Quests)
                .ThenInclude(pq => pq.Quest)
            .Include(p => p.Quests)
                .ThenInclude(pq => pq.Entries)
            .FirstAsync(p => p.Id == playerId);

        var quests = player.Quests.Select(q => new QuestDto
        {
            Id = q.Quest.Id,
            Title = q.Quest.Title,
            CurrentStage = q.CurrentStage,
            Status = q.Status.ToString(),

            Entries = q.Entries
                .OrderBy(e => e.Stage)
                .Select(e => new QuestEntryDto
                {
                    Stage = e.Stage,
                    Text = e.Text
                })
                .ToList()
        });

        return Ok(quests);
    }
}
