using Backend.Models.Game;
using Backend.GameWorld.Quests;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public static class GameDbSeeder
{
    public static async Task SeedAsync(GameDbContext context)
    {
        if (!await context.Items.AnyAsync())
        {
            var items = new List<Item>
            {
                new Item
                {
                    Code = "knife_basic",
                    Name = "Nóż",
                    Description = "Zadaje 7–11 obrażeń.",
                    Icon = "/icons/items/knife.png",
                    ItemType = ItemType.Equipment,
                    Slot = EquipmentSlot.Weapon,
                    MinDamage = 7,
                    MaxDamage = 11
                },
                new Item
                {
                    Code = "bandage_basic",
                    Name = "Bandaż",
                    Description = "Przywraca 20 HP.",
                    Icon = "/icons/items/bandage.png",
                    ItemType = ItemType.Consumable,
                    HealAmount = 20
                },
                new Item
                {
                    Code = "flashlight_basic",
                    Name = "Latarka",
                    Description = "Oświetla ciemne miejsca.",
                    Icon = "/icons/items/flashlight.png",
                    ItemType = ItemType.Equipment
                },
                new Item
                {
                    Code = "forest_herb",
                    Name = "Rzadkie ziele",
                    Description = "Przedmiot questowy.",
                    Icon = "/icons/itemsquest/forestherb.png",
                    ItemType = ItemType.QuestItem
                }
            };

            context.Items.AddRange(items);
        }

        if (!await context.Quests.AnyAsync())
        {
            context.Quests.AddRange(
                QuestDefinitions.EscapeIsland,
                QuestDefinitions.CaveMystery,
                QuestDefinitions.Settlement
            );
        }

        await context.SaveChangesAsync();
    }
}
