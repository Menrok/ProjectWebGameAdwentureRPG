using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers;

[ApiController]
[Route("api/game/equipment")]
[Authorize]
public class EquipmentController : ControllerBase
{
    private readonly EquipmentService _equipmentService;

    public EquipmentController(EquipmentService equipmentService)
    {
        _equipmentService = equipmentService;
    }

    [HttpPost("equip/{inventoryItemId}")]
    public async Task<IActionResult> EquipItem(int inventoryItemId)
    {
        var userId = GetUserId();
        await _equipmentService.EquipItem(userId, inventoryItemId);
        return Ok(new { success = true });
    }

    [HttpPost("unequip/{inventoryItemId}")]
    public async Task<IActionResult> UnequipItem(int inventoryItemId)
    {
        var userId = GetUserId();
        await _equipmentService.UnequipItem(userId, inventoryItemId);
        return Ok(new { success = true });
    }

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
