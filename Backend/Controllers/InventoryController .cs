using Backend.Services.Game;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("{playerId}")]
    public async Task<IActionResult> GetInventory(int playerId)
    {
        var inventory = await _inventoryService.GetInventory(playerId);
        return Ok(inventory);
    }

    [HttpPost("{playerId}/add/{itemId}")]
    public async Task<IActionResult> AddItemToInventory(int playerId, int itemId)
    {
        await _inventoryService.AddItemToInventory(playerId, itemId);
        return Ok();
    }

    [HttpPost("{playerId}/equip/weapon/{inventoryItemId}")]
    public async Task<IActionResult> EquipWeapon(int playerId, int inventoryItemId)
    {
        await _inventoryService.EquipWeapon(playerId, inventoryItemId);
        return Ok();
    }

    [HttpPost("{playerId}/equip/clothing/{inventoryItemId}")]
    public async Task<IActionResult> EquipClothing(int playerId, int inventoryItemId)
    {
        await _inventoryService.EquipClothing(playerId, inventoryItemId);
        return Ok();
    }

    [HttpPost("{playerId}/unequip/weapon")]
    public async Task<IActionResult> UnequipWeapon(int playerId)
    {
        await _inventoryService.UnequipWeapon(playerId);
        return Ok();
    }

    [HttpPost("{playerId}/unequip/clothing")]
    public async Task<IActionResult> UnequipClothing(int playerId)
    {
        await _inventoryService.UnequipClothing(playerId);
        return Ok();
    }

    [HttpPost("{playerId}/use/{inventoryItemId}")]
    public async Task<IActionResult> UseConsumable(int playerId, int inventoryItemId)
    {
        await _inventoryService.UseConsumable(playerId, inventoryItemId);
        return Ok();
    }

    [HttpGet("{playerId}/status")]
    public async Task<IActionResult> GetPlayerStatus(int playerId)
    {
        var status = await _inventoryService.GetPlayerStatus(playerId);
        return Ok(status);
    }
}
