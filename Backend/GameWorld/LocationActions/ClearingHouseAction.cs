using Backend.Models.Game;
using Backend.DTOs.Game;
using Backend.Services;
using Backend.Data;

namespace Backend.GameWorld.LocationActions;

public class ClearingHouseAction
{
    private readonly GameDbContext _context;
    private readonly InventoryService _inventoryService;

    public ClearingHouseAction(GameDbContext context, InventoryService inventoryService)
    {
        _context = context;
        _inventoryService = inventoryService;
    }

    public ActionResultDto Execute(Player player, string actionId)
    {
        return actionId switch
        {
            "knock_door" => KnockDoor(player),
            "talk_to_npc" => TalkToNpc(player),
            "give_forest_herb" => GiveForestHerb(player),
            _ => new ActionResultDto
            {
                Text = "Nie możesz tego teraz zrobić."
            }
        };
    }

    public List<LocationAction> GetAvailableActions(Player player)
    {
        var actions = new List<LocationAction>();

        if (!player.Flags.Contains("clearing_house_knocked"))
        {
            actions.Add(new()
            {
                Id = "knock_door",
                Text = "Zapukaj do drzwi"
            });
            return actions;
        }

        if (
            player.Flags.Contains("npc_requested_help") &&
            _inventoryService.HasItem(player.Id, "forest_herb") &&
            !player.Flags.Contains("npc_help_done")
        )
        {
            actions.Add(new()
            {
                Id = "give_forest_herb",
                Text = "Oddaj ziele"
            });
            return actions;
        }

        actions.Add(new()
        {
            Id = "talk_to_npc",
            Text = "Porozmawiaj z mieszkańcem"
        });

        return actions;
    }

    private ActionResultDto KnockDoor(Player player)
    {
        player.Flags.Add("clearing_house_knocked");

        return new ActionResultDto
        {
            Text =
                "Pukasz do drzwi.\n\n" +
                "Długa cisza.\n\n" +
                "W końcu słyszysz kroki.\n" +
                "Drzwi uchylają się tylko na tyle, by ktoś mógł cię zobaczyć.\n\n" +
                "— Nie wyglądasz na kogoś stąd — mówi mężczyzna."
        };
    }

    private ActionResultDto TalkToNpc(Player player)
    {
        if (player.Flags.Contains("npc_help_done"))
        {
            return new ActionResultDto
            {
                Text =
                    "— Uważaj na siebie — mówi.\n" +
                    "— Ta wyspa nie daje drugich szans."
            };
        }

        if (!player.Flags.Contains("npc_requested_help"))
        {
            player.Flags.Add("npc_requested_help");

            return new ActionResultDto
            {
                Text =
                    "Mężczyzna mierzy cię wzrokiem.\n\n" +
                    "— Jeśli chcesz informacji, musisz najpierw pomóc.\n\n" +
                    "— W lesie rośnie rzadkie ziele. Potrzebuję go.\n" +
                    "— Przynieś mi je, a pogadamy."
            };
        }

        return new ActionResultDto
        {
            Text =
                "— Wciąż czekam — mówi krótko.\n" +
                "— Bez przysługi nie ma rozmowy."
        };
    }

    private ActionResultDto GiveForestHerb(Player player)
    {
        var herb = _context.Items.First(i => i.Code == "forest_herb");
        var knife = _context.Items.First(i => i.Code == "knife_basic");

        _inventoryService.RemoveItemFromInventory(player.Id, herb.Id)
            .GetAwaiter().GetResult();

        _inventoryService.AddItemToInventory(player.Id, knife.Id)
            .GetAwaiter().GetResult();

        player.Flags.Add("npc_help_done");
        player.Flags.Add("village_discovered");

        return new ActionResultDto
        {
            Text =
                "Mężczyzna bierze ziele i długo mu się przygląda.\n\n" +
                "— Dobra robota.\n\n" +
                "— Na północ stąd jest wioska.\n" +
                "— A to… przyda ci się.\n\n" +
                "Podaje ci nóż.\n\n" +
                "— Ta wyspa nie wybacza błędów.",
            Items =
            {
                new()
                {
                    Code = knife.Code,
                    Name = knife.Name,
                    Icon = knife.Icon
                }
            },
            DiscoveredLocations =
            {
                "village"
            }
        };
    }
}
