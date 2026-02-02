using Backend.Data;
using Backend.DTOs.Game;
using Backend.Models.Game;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class PlayerStatsService
{
    private readonly GameDbContext _context;

    public PlayerStatsService(GameDbContext context)
    {
        _context = context;
    }

    public async Task<PlayerStatusDto> GetPlayerStatus(int userId)
    {
        var player = await _context.Players
            .Include(p => p.Inventory)
                .ThenInclude(ii => ii.Item)
            .FirstAsync(p => p.UserId == userId);

        var equippedItems = player.Inventory
            .Where(i => i.IsEquipped)
            .Select(i => i.Item)
            .ToList();

        var minAttack = player.BaseMinAttack;
        var maxAttack = player.BaseMaxAttack;
        var defense = player.BaseDefense;

        foreach (var item in equippedItems)
        {
            if (item.MinDamage.HasValue)
                minAttack += item.MinDamage.Value;

            if (item.MaxDamage.HasValue)
                maxAttack += item.MaxDamage.Value;

            defense += item.DefenseBonus;
        }

        return new PlayerStatusDto
        {
            PlayerId = player.Id,
            Name = player.Name,

            Health = player.Health,
            MaxHealth = player.MaxHealth,

            MinAttack = minAttack,
            MaxAttack = maxAttack,
            Defense = defense,

            Crystals = player.Crystals,

            InventoryCount = player.Inventory.Count(i => !i.IsEquipped),

            EquippedItems = equippedItems
                .Where(i => i.Slot != null)
                .Select(i => new EquippedItemDto
                {
                    Slot = i.Slot!.Value,
                    Name = i.Name,
                    Icon = i.Icon
                })
                .ToList()
        };
    }
}
