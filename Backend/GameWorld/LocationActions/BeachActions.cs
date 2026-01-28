using Backend.Data;
using Backend.Models.Game;
using Backend.DTOs.Game;
using Backend.Services;

namespace Backend.GameWorld.LocationActions;

public class BeachAction
{
    private readonly GameDbContext _context;
    private readonly InventoryService _inventoryService;

    public BeachAction(
        GameDbContext context,
        InventoryService inventoryService)
    {
        _context = context;
        _inventoryService = inventoryService;
    }

    public ActionResultDto Execute(Player player, string actionId)
    {
        return actionId switch
        {
            "search_beach" => SearchBeach(player),
            "look_around" => LookAround(player),
            "check_shore" => CheckShore(player),
            _ => new ActionResultDto
            {
                Text = "Nie możesz tego teraz zrobić."
            }
        };
    }

    public List<LocationAction> GetAvailableActions(Player player)
    {
        return new()
        {
            new() { Id = "search_beach", Text = "Przeszukaj plażę" },
            new() { Id = "check_shore", Text = "Sprawdź brzeg" },
            new() { Id = "look_around", Text = "Rozejrzyj się" }
        };
    }

    private ActionResultDto SearchBeach(Player player)
    {
        if (player.Flags.Contains("beach_searched"))
        {
            return new ActionResultDto
            {
                Text = "Przeszukałaś już plażę. Nie znajdujesz niczego nowego."
            };
        }

        player.Flags.Add("beach_searched");

        var bandage = _context.Items.First(i => i.Code == "bandage_basic");
        var clothes = _context.Items.First(i => i.Code == "clothing_basic");

        _inventoryService.AddItemToInventory(player.Id, bandage.Id)
            .GetAwaiter().GetResult();

        _inventoryService.AddItemToInventory(player.Id, clothes.Id)
            .GetAwaiter().GetResult();

        return new ActionResultDto
        {
            Text =
                "Przeszukujesz plażę.\n\n" +
                "Wśród mokrego piasku znajdujesz bandaż oraz ubranie.",
            Items =
            {
                new()
                {
                    Code = bandage.Code,
                    Name = bandage.Name,
                    Icon = bandage.Icon
                },
                new()
                {
                    Code = clothes.Code,
                    Name = clothes.Name,
                    Icon = clothes.Icon
                }
            }
        };
    }

    private ActionResultDto LookAround(Player player)
    {
        if (player.Flags.Contains("forest_discovered"))
        {
            return new ActionResultDto
            {
                Text = "Już wiesz, że jedyna droga prowadzi w głąb wyspy."
            };
        }

        player.Flags.Add("forest_discovered");

        return new ActionResultDto
        {
            Text =
                "Rozglądasz się uważnie.\n\n" +
                "Plaża kończy się gęstym lasem. Między drzewami widać wąską ścieżkę.",
            DiscoveredLocations = { "forest" }
        };
    }

    private ActionResultDto CheckShore(Player player)
    {
        if (player.Flags.Contains("shore_checked"))
        {
            return new ActionResultDto
            {
                Text = "Nic się nie zmieniło. Tylko morze i cisza."
            };
        }

        player.Flags.Add("shore_checked");

        return new ActionResultDto
        {
            Text =
                "Idziesz wzdłuż linii wody.\n\n" +
                "Nie ma śladów innych ocalałych. Wygląda na to, że jesteś tu sama."
        };
    }
}
