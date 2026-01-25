using Backend.Data;
using Backend.Models.Game;
using Backend.DTOs.Game;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Game;

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
        return await _context.InventoryItems.Include(ii => ii.Item).Where(ii => ii.PlayerId == playerId).ToListAsync();
    }

    public async Task AddItemToInventory(int playerId, int itemId)
    {
        var player = await _context.Players.Include(p => p.Inventory).FirstOrDefaultAsync(p => p.Id == playerId);

        if (player == null)
            throw new Exception("Player not found");

        if (player.Inventory.Count >= MaxInventorySize)
            throw new Exception("Inventory is full");

        var item = await _context.Items.FindAsync(itemId);
        if (item == null)
            throw new Exception("Item not found");

        var inventoryItem = new InventoryItem
        {
            PlayerId = playerId,
            ItemId = itemId
        };

        _context.InventoryItems.Add(inventoryItem);
        await _context.SaveChangesAsync();
    }

    public Task EquipWeapon(int playerId, int inventoryItemId) => EquipItem(playerId, inventoryItemId, EquipmentSlot.Weapon);

    public Task EquipClothing(int playerId, int inventoryItemId) => EquipItem(playerId, inventoryItemId, EquipmentSlot.Clothing);

    private async Task EquipItem(int playerId, int inventoryItemId, EquipmentSlot slot)
    {
        var inventoryItem = await _context.InventoryItems
            .Include(ii => ii.Item)
            .Include(ii => ii.Player)
            .FirstOrDefaultAsync(ii => 
                ii.Id == inventoryItemId && 
                ii.PlayerId == playerId);

        if (inventoryItem == null)
            throw new Exception("Item not found in inventory");

        var item = inventoryItem.Item;
        var player = inventoryItem.Player;

        if (item.ItemType != ItemType.Equipment)
            throw new Exception("Item is not equipment");

        if (item.Slot != slot)
            throw new Exception("Item cannot be equipped in this slot");

        if (slot == EquipmentSlot.Weapon && player.EquippedWeaponId != null)
            throw new Exception("Weapon slot already occupied");

        if (slot == EquipmentSlot.Clothing && player.EquippedClothingId != null)
            throw new Exception("Clothing slot already occupied");

        _context.InventoryItems.Remove(inventoryItem);

        if (slot == EquipmentSlot.Weapon)
            player.EquippedWeaponId = item.Id;
        else
            player.EquippedClothingId = item.Id;

        await _context.SaveChangesAsync();
    }

    public async Task UnequipWeapon(int playerId) => await UnequipItem(playerId, EquipmentSlot.Weapon);

    public async Task UnequipClothing(int playerId) => await UnequipItem(playerId, EquipmentSlot.Clothing);

    private async Task UnequipItem(int playerId, EquipmentSlot slot)
    {
        var player = await _context.Players.Include(p => p.Inventory).FirstOrDefaultAsync(p => p.Id == playerId);

        if (player == null)
            throw new Exception("Player not found");

        if (player.Inventory.Count >= MaxInventorySize)
            throw new Exception("Inventory is full");

        int? itemId = slot switch
        {
            EquipmentSlot.Weapon => player.EquippedWeaponId,
            EquipmentSlot.Clothing => player.EquippedClothingId,
            _ => null
        };

        if (itemId == null)
            throw new Exception("Nothing equipped in this slot");

        _context.InventoryItems.Add(new InventoryItem
        {
            PlayerId = playerId,
            ItemId = itemId.Value
        });

        if (slot == EquipmentSlot.Weapon)
            player.EquippedWeaponId = null;
        else
            player.EquippedClothingId = null;

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
            throw new Exception("Item not found in inventory");

        var item = inventoryItem.Item;
        var player = inventoryItem.Player;

        if (item.ItemType != ItemType.Consumable)
            throw new Exception("Item is not consumable");

        player.Health = Math.Min(
            player.MaxHealth,
            player.Health + item.HealAmount
        );

        _context.InventoryItems.Remove(inventoryItem);
        await _context.SaveChangesAsync();
    }

    public async Task<PlayerStatusDto> GetPlayerStatus(int playerId)
    {
        var player = await _context.Players
            .Include(p => p.Inventory)
            .Include(p => p.EquippedWeapon)
            .Include(p => p.EquippedClothing)
            .FirstOrDefaultAsync(p => p.Id == playerId);

        if (player == null)
            throw new Exception("Player not found");

        var attack =  player.BaseAttack + (player.EquippedWeapon?.AttackBonus ?? 0);

        var defense = player.BaseDefense + (player.EquippedClothing?.DefenseBonus ?? 0);

        return new PlayerStatusDto
        {
            PlayerId = player.Id,
            Name = player.Name,

            Health = player.Health,
            MaxHealth = player.MaxHealth,

            Attack = attack,
            Defense = defense,

            Weapon = player.EquippedWeapon?.Name,
            Clothing = player.EquippedClothing?.Name,

            InventoryCount = player.Inventory.Count
        };
    }
}
