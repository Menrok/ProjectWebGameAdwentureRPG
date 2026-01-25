using Backend.Models.Game;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public static class GameDbSeeder
{
    public static async Task SeedAsync(GameDbContext context)
    {
        if (await context.Items.AnyAsync())
            return;

        var items = new List<Item>
        {
            new Item
            {
                Name = "Stary Miecz",
                ItemType = ItemType.Equipment,
                Slot = EquipmentSlot.Weapon,
                AttackBonus = 5
            },
            new Item
            {
                Name = "Proste Ubranie",
                ItemType = ItemType.Equipment,
                Slot = EquipmentSlot.Clothing,
                DefenseBonus = 3
            },
            new Item
            {
                Name = "Mała Mikstura Zdrowia",
                ItemType = ItemType.Consumable,
                HealAmount = 20
            }
        };

        context.Items.AddRange(items);
        await context.SaveChangesAsync();
    }
}
