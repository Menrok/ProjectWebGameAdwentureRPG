using Backend.Data;
using Backend.DTOs.Game;
using Backend.Models.Game;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class InventoryService
{
    private const int MaxInventorySize = 10;
    private readonly GameDbContext _context;

    public InventoryService(GameDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<InventoryItem>> GetInventory(int playerId)
    {
        return await _context.InventoryItems
            .Include(ii => ii.Item)
            .Where(ii => ii.PlayerId == playerId)
            .ToListAsync();
    }

    public async Task AddItemToInventory(int playerId, int itemId)
    {
        var slot = await GetFreeSlot(playerId);

        _context.InventoryItems.Add(new InventoryItem
        {
            PlayerId = playerId,
            ItemId = itemId,
            SlotIndex = slot,
            IsEquipped = false
        });

        await _context.SaveChangesAsync();
    }

    public async Task UseConsumable(int playerId, int inventoryItemId)
    {
        var inventoryItem = await _context.InventoryItems
            .Include(ii => ii.Item)
            .Include(ii => ii.Player)
            .FirstAsync(ii => ii.Id == inventoryItemId && ii.PlayerId == playerId);
            
        if (inventoryItem.Item.ItemType != ItemType.Consumable)
            throw new InvalidOperationException("Tego przedmiotu nie można użyć.");

        inventoryItem.Player.Health = Math.Min(
            inventoryItem.Player.MaxHealth,
            inventoryItem.Player.Health + inventoryItem.Item.HealAmount
        );

        _context.InventoryItems.Remove(inventoryItem);
        await _context.SaveChangesAsync();
    }

    public bool HasItem(int playerId, string itemCode)
    {
        return _context.InventoryItems
            .Include(ii => ii.Item)
            .Any(ii => ii.PlayerId == playerId && ii.Item.Code == itemCode);
    }

    private async Task<int> GetFreeSlot(int playerId)
    {
        var occupied = await _context.InventoryItems
            .Where(ii => ii.PlayerId == playerId && !ii.IsEquipped)
            .Select(ii => ii.SlotIndex!.Value)
            .ToListAsync();

        for (int i = 0; i < MaxInventorySize; i++)
            if (!occupied.Contains(i))
                return i;

        throw new Exception("Inventory full");
    }
}
