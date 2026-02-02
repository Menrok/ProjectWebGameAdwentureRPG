using Backend.Models.Game;
using Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class EquipmentService
{
    private readonly GameDbContext _context;

    public EquipmentService(GameDbContext context)
    {
        _context = context;
    }

    public async Task EquipItem(int playerId, int inventoryItemId)
    {
        var invItem = await _context.InventoryItems
            .Include(i => i.Item)
            .FirstAsync(i => i.Id == inventoryItemId && i.PlayerId == playerId);

        if (invItem.Item.Slot == null)
            throw new InvalidOperationException("Ten przedmiot nie jest elementem ekwipunku.");

        if (invItem.Item.ItemType != ItemType.Equipment)
            throw new InvalidOperationException("Tylko equipment może być zakładany.");

        var slot = invItem.Item.Slot.Value;

        var equippedInSlot = await _context.InventoryItems
            .Include(i => i.Item)
            .Where(i =>
                i.PlayerId == playerId &&
                i.IsEquipped &&
                i.Item.Slot == slot)
            .ToListAsync();

        foreach (var item in equippedInSlot)
            item.IsEquipped = false;

        invItem.IsEquipped = true;
        invItem.SlotIndex = null;

        await _context.SaveChangesAsync();
    }

    public async Task UnequipItem(int playerId, int inventoryItemId)
    {
        var invItem = await _context.InventoryItems.FirstAsync(i => i.Id == inventoryItemId && i.PlayerId == playerId);

        if (!invItem.IsEquipped)
            return;

        invItem.IsEquipped = false;
        invItem.SlotIndex = await GetFreeSlot(playerId);

        await _context.SaveChangesAsync();
    }

    private async Task<int> GetFreeSlot(int playerId)
    {
        var occupied = await _context.InventoryItems
            .Where(ii => ii.PlayerId == playerId && !ii.IsEquipped)
            .Select(ii => ii.SlotIndex!.Value)
            .ToListAsync();

        for (int i = 0; i < 10; i++)
            if (!occupied.Contains(i))
                return i;

        throw new InvalidOperationException("Inventory full");
    }
}
