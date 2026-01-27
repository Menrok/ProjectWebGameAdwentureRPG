using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers.Game;

[ApiController]
[Route("api/game/inventory")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly InventoryService _inventoryService;

    public InventoryController(InventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetInventory()
    {
        var playerId = GetPlayerId();
        var inventory = await _inventoryService.GetInventory(playerId);
        return Ok(inventory);
    }

    [HttpPost("add/{itemId}")]
    public async Task<IActionResult> AddItemToInventory(int itemId)
    {
        var playerId = GetPlayerId();
        await _inventoryService.AddItemToInventory(playerId, itemId);
        return Ok();
    }

    [HttpPost("equip/weapon/{inventoryItemId}")]
    public async Task<IActionResult> EquipWeapon(int inventoryItemId)
    {
        var playerId = GetPlayerId();
        await _inventoryService.EquipWeapon(playerId, inventoryItemId);
        return Ok();
    }

    [HttpPost("equip/clothing/{inventoryItemId}")]
    public async Task<IActionResult> EquipClothing(int inventoryItemId)
    {
        var playerId = GetPlayerId();
        await _inventoryService.EquipClothing(playerId, inventoryItemId);
        return Ok();
    }

    [HttpPost("unequip/weapon")]
    public async Task<IActionResult> UnequipWeapon()
    {
        var playerId = GetPlayerId();
        await _inventoryService.UnequipWeapon(playerId);
        return Ok();
    }

    [HttpPost("unequip/clothing")]
    public async Task<IActionResult> UnequipClothing()
    {
        var playerId = GetPlayerId();
        await _inventoryService.UnequipClothing(playerId);
        return Ok();
    }

    [HttpPost("use/{inventoryItemId}")]
    public async Task<IActionResult> UseConsumable(int inventoryItemId)
    {
        var playerId = GetPlayerId();
        await _inventoryService.UseConsumable(playerId, inventoryItemId);
        return Ok(new { success = true });
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetPlayerStatus()
    {
        var playerId = GetPlayerId();
        var status = await _inventoryService.GetPlayerStatus(playerId);
        return Ok(status);
    }

    private int GetPlayerId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
