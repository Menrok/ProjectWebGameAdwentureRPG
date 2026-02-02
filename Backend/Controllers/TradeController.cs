using Backend.Services;
using Backend.DTOs.Game;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers;

[ApiController]
[Route("api/game/trade")]
[Authorize]
public class TradeController : ControllerBase
{
    private readonly TradeService _tradeService;

    public TradeController(TradeService tradeService)
    {
        _tradeService = tradeService;
    }

    [HttpGet("{tradeId}")]
    public IActionResult GetTrade(string tradeId)
    {
        var items = _tradeService.GetTradeItems(tradeId);
        return Ok(items);
    }

    [HttpPost("buy")]
    public async Task<IActionResult> BuyItem([FromBody] BuyTradeItemRequest request)
    {
        var userId = GetUserId();

        try
        {
            var crystalsLeft = await _tradeService.BuyItem(
                userId,
                request.TradeId,
                request.ItemCode
            );

            return Ok(new
            {
                success = true,
                crystalsLeft
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
