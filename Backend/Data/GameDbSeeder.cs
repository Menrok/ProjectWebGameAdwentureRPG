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
                    Code = "knife_basic",
                    Name = "Nóż",
                    Description = "Zadaje 3 obrażenia.",
                    Icon = "/icons/items/knife.png",
                    ItemType = ItemType.Weapon,
                    Slot = EquipmentSlot.Weapon,
                    AttackBonus = 3
                },
                new Item
                {
                    Code = "clothing_basic",
                    Name = "Proste ubranie",
                    Description = "Daje 3 pancerza.",
                    Icon = "/icons/items/clothing.png",
                    ItemType = ItemType.Clothing,
                    Slot = EquipmentSlot.Clothing,
                    DefenseBonus = 3
                },
                new Item
                {
                    Code = "bandage_basic",
                    Name = "Bandaż",
                    Description = "Przywraca 20 HP.",
                    Icon = "/icons/items/bandage.png",
                    ItemType = ItemType.Consumable,
                    HealAmount = 20
                }
            };

        context.Items.AddRange(items);
        await context.SaveChangesAsync();
    }
}
