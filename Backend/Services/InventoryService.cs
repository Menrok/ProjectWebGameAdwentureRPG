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

    public async Task EquipWeapon(int playerId, int inventoryItemId) => await EquipItem(playerId, inventoryItemId, EquipmentSlot.Weapon);

    public async Task EquipClothing(int playerId, int inventoryItemId) => await EquipItem(playerId, inventoryItemId, EquipmentSlot.Clothing);

    private async Task EquipItem(int playerId, int inventoryItemId, EquipmentSlot slot)
    {
        var inventoryItem = await _context.InventoryItems
            .Include(ii => ii.Item)
            .FirstOrDefaultAsync(ii =>
                ii.Id == inventoryItemId &&
                ii.PlayerId == playerId &&
                !ii.IsEquipped);

        if (inventoryItem == null)
            throw new Exception("Item not found");

        if (inventoryItem.Item.Slot != slot)
            throw new Exception("Wrong slot");

        var player = await _context.Players
            .FirstAsync(p => p.Id == playerId);

        if (slot == EquipmentSlot.Weapon && player.EquippedWeaponId != null)
        {
            await UnequipItem(playerId, EquipmentSlot.Weapon);
        }

        if (slot == EquipmentSlot.Clothing && player.EquippedClothingId != null)
        {
            await UnequipItem(playerId, EquipmentSlot.Clothing);
        }

        inventoryItem.IsEquipped = true;
        inventoryItem.SlotIndex = null;

        if (slot == EquipmentSlot.Weapon)
            player.EquippedWeaponId = inventoryItem.ItemId;

        if (slot == EquipmentSlot.Clothing)
            player.EquippedClothingId = inventoryItem.ItemId;

        await _context.SaveChangesAsync();
    }

    public async Task UnequipWeapon(int playerId) => await UnequipItem(playerId, EquipmentSlot.Weapon);

    public async Task UnequipClothing(int playerId) => await UnequipItem(playerId, EquipmentSlot.Clothing);

    private async Task UnequipItem(int playerId, EquipmentSlot slot)
    {
        var player = await _context.Players
            .FirstAsync(p => p.Id == playerId);

        int? itemId = slot switch
        {
            EquipmentSlot.Weapon => player.EquippedWeaponId,
            EquipmentSlot.Clothing => player.EquippedClothingId,
            _ => null
        };

        if (itemId == null)
            return;

        var inventoryItem = await _context.InventoryItems
            .FirstAsync(ii =>
                ii.PlayerId == playerId &&
                ii.ItemId == itemId &&
                ii.IsEquipped);

        var occupiedSlots = await _context.InventoryItems
            .Where(ii => ii.PlayerId == playerId && ii.SlotIndex != null)
            .Select(ii => ii.SlotIndex!.Value)
            .ToListAsync();

        int freeSlot = Enumerable.Range(0, MaxInventorySize)
            .First(i => !occupiedSlots.Contains(i));

        inventoryItem.IsEquipped = false;
        inventoryItem.SlotIndex = freeSlot;

        if (slot == EquipmentSlot.Weapon)
            player.EquippedWeaponId = null;

        if (slot == EquipmentSlot.Clothing)
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
            throw new Exception("Item not found");

        inventoryItem.Player.Health = Math.Min(
            inventoryItem.Player.MaxHealth,
            inventoryItem.Player.Health + inventoryItem.Item.HealAmount
        );

        _context.InventoryItems.Remove(inventoryItem);
        await _context.SaveChangesAsync();
    }

    public async Task<PlayerStatusDto> GetPlayerStatus(int playerId)
    {
        var player = await _context.Players
            .FirstOrDefaultAsync(p => p.Id == playerId);

        if (player == null)
            throw new Exception("Player not found");

        var equippedItems = await _context.InventoryItems
            .Include(ii => ii.Item)
            .Where(ii =>
                ii.PlayerId == playerId &&
                ii.IsEquipped)
            .ToListAsync();

        var weapon = equippedItems
            .FirstOrDefault(ii => ii.Item.Slot == EquipmentSlot.Weapon)
            ?.Item;

        var clothing = equippedItems
            .FirstOrDefault(ii => ii.Item.Slot == EquipmentSlot.Clothing)
            ?.Item;

        return new PlayerStatusDto
        {
            PlayerId = player.Id,
            Name = player.Name,

            Health = player.Health,
            MaxHealth = player.MaxHealth,

            Attack = player.BaseAttack + (weapon?.AttackBonus ?? 0),
            Defense = player.BaseDefense + (clothing?.DefenseBonus ?? 0),

            Weapon = weapon?.Icon,
            Clothing = clothing?.Icon,

            InventoryCount = await _context.InventoryItems
                .CountAsync(ii => ii.PlayerId == playerId && !ii.IsEquipped)
        };
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
