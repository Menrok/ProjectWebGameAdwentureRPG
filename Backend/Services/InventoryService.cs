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
        var inventoryCount = await _context.InventoryItems
            .CountAsync(ii => ii.PlayerId == playerId);

        if (inventoryCount >= MaxInventorySize)
            throw new Exception("Inventory is full");

        var item = await _context.Items.FindAsync(itemId);
        if (item == null)
            throw new Exception("Item not found");

        var existing = await _context.InventoryItems.FirstOrDefaultAsync(ii =>
            ii.PlayerId == playerId &&
            ii.ItemId == itemId &&
            !ii.IsEquipped);

        if (existing != null)
        {
            existing.Quantity += 1;
        }
        else
        {
            _context.InventoryItems.Add(new InventoryItem
            {
                PlayerId = playerId,
                ItemId = itemId
            });
        }

        await _context.SaveChangesAsync();
    }

    public async Task EquipWeapon(int playerId, int inventoryItemId) => await EquipItem(playerId, inventoryItemId, EquipmentSlot.Weapon);

    public async Task EquipClothing(int playerId, int inventoryItemId) => await EquipItem(playerId, inventoryItemId, EquipmentSlot.Clothing);

    private async Task EquipItem(int playerId, int inventoryItemId, EquipmentSlot slot)
    {
        var inventoryItem = await _context.InventoryItems
            .Include(ii => ii.Item)
            .FirstOrDefaultAsync(ii =>
                ii.Id == inventoryItemId &&
                ii.PlayerId == playerId);

        if (inventoryItem == null)
            throw new Exception("Item not found in inventory");

        if (inventoryItem.IsEquipped)
            throw new Exception("Item already equipped");

        if (inventoryItem.Item.ItemType != ItemType.Equipment)
            throw new Exception("Item is not equipment");

        if (inventoryItem.Item.Slot != slot)
            throw new Exception("Item cannot be equipped in this slot");

        var equippedInSlot = await _context.InventoryItems
            .Include(ii => ii.Item)
            .FirstOrDefaultAsync(ii =>
                ii.PlayerId == playerId &&
                ii.IsEquipped &&
                ii.Item.Slot == slot);

        if (equippedInSlot != null)
            equippedInSlot.IsEquipped = false;

        inventoryItem.IsEquipped = true;

        await _context.SaveChangesAsync();
    }

    public async Task UnequipWeapon(int playerId) => await UnequipItem(playerId, EquipmentSlot.Weapon);

    public async Task UnequipClothing(int playerId) => await UnequipItem(playerId, EquipmentSlot.Clothing);

    private async Task UnequipItem(int playerId, EquipmentSlot slot)
    {
        var equipped = await _context.InventoryItems
            .Include(ii => ii.Item)
            .FirstOrDefaultAsync(ii =>
                ii.PlayerId == playerId &&
                ii.IsEquipped &&
                ii.Item.Slot == slot);

        if (equipped == null)
            throw new Exception("Nothing equipped in this slot");

        equipped.IsEquipped = false;

        await _context.SaveChangesAsync();
    }

    public async Task UseConsumable(int playerId, int inventoryItemId)
    {
        var inventoryItem = await _context.InventoryItems
            .Include(ii => ii.Item)
            .Include(ii => ii.Player)
            .FirstOrDefaultAsync(ii =>
                ii.Id == inventoryItemId &&
                ii.PlayerId == playerId);

        if (inventoryItem == null)
            throw new Exception("Item not found");

        if (inventoryItem.Item.ItemType != ItemType.Consumable)
            throw new Exception("Item is not consumable");

        var player = inventoryItem.Player;

        player.Health = Math.Min(
            player.MaxHealth,
            player.Health + inventoryItem.Item.HealAmount
        );

        inventoryItem.Quantity -= 1;

        if (inventoryItem.Quantity <= 0)
            _context.InventoryItems.Remove(inventoryItem);

        await _context.SaveChangesAsync();
    }

    public async Task<PlayerStatusDto> GetPlayerStatus(int playerId)
    {
        var player = await _context.Players
            .Include(p => p.Inventory)
                .ThenInclude(ii => ii.Item)
            .FirstOrDefaultAsync(p => p.Id == playerId);

        if (player == null)
            throw new Exception("Player not found");

        var weapon = player.Inventory
            .FirstOrDefault(ii => ii.IsEquipped && ii.Item.Slot == EquipmentSlot.Weapon);

        var clothing = player.Inventory
            .FirstOrDefault(ii => ii.IsEquipped && ii.Item.Slot == EquipmentSlot.Clothing);

        var attack = player.BaseAttack + (weapon?.Item.AttackBonus ?? 0);
        var defense = player.BaseDefense + (clothing?.Item.DefenseBonus ?? 0);

        return new PlayerStatusDto
        {
            PlayerId = player.Id,
            Name = player.Name,

            Health = player.Health,
            MaxHealth = player.MaxHealth,

            Attack = attack,
            Defense = defense,

            Weapon = weapon?.Item.Name,
            Clothing = clothing?.Item.Name,

            InventoryCount = player.Inventory.Count
        };
    }
}
