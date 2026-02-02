using Backend.Services;
using Backend.DTOs.Game;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers;

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
        var userId = GetUserId();
        var inventory = await _inventoryService.GetInventory(userId);

        var dto = inventory.Select(ii => new InventoryItemDto
        {
            Id = ii.Id,
            SlotIndex = ii.SlotIndex,
            IsEquipped = ii.IsEquipped,
            Item = new ItemDto
            {
                Id = ii.Item.Id,
                Name = ii.Item.Name,
                Description = ii.Item.Description,
                Icon = ii.Item.Icon,
                ItemType = ii.Item.ItemType,
                Slot = ii.Item.Slot,
                MinDamage = ii.Item.MinDamage,
                MaxDamage = ii.Item.MaxDamage,
                DefenseBonus = ii.Item.DefenseBonus,
                HealAmount = ii.Item.HealAmount
            }
        });

        return Ok(dto);
    }

    [HttpPost("add/{itemId}")]
    public async Task<IActionResult> AddItemToInventory(int itemId)
    {
        var userId = GetUserId();
        await _inventoryService.AddItemToInventory(userId, itemId);
        return Ok(new { success = true });
    }

    [HttpPost("use/{inventoryItemId}")]
    public async Task<IActionResult> UseConsumable(int inventoryItemId)
    {
        var userId = GetUserId();
        await _inventoryService.UseConsumable(userId, inventoryItemId);
        return Ok(new { success = true });
    }

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
