using Backend.Data;
using Backend.Models.Game;
using Backend.DTOs.Game;
using Backend.Services;

namespace Backend.GameWorld.LocationActions;

public class ForestAction
{
    private readonly GameDbContext _context;
    private readonly InventoryService _inventoryService;

    public ForestAction(GameDbContext context, InventoryService inventoryService)
    {
        _context = context;
        _inventoryService = inventoryService;
    }

    public ActionResultDto Execute(Player player, string actionId)
    {
        return actionId switch
        {
            "explore_paths" => ExplorePaths(player),
            "look_around_forest" => LookAround(player),
            "inspect_tracks" => InspectTracks(player),
            "collect_forest_herb" => CollectForestHerb(player),
            _ => new ActionResultDto
            {
                Text = "Nie możesz tego teraz zrobić."
            }
        };
    }

    public List<LocationAction> GetAvailableActions(Player player)
    {
        var actions = new List<LocationAction>
        {
            new() { Id = "explore_paths", Text = "Zbadaj stare ścieżki" },
            new() { Id = "look_around_forest", Text = "Rozejrzyj się" },
            new() { Id = "inspect_tracks", Text = "Zbadaj ślady na ziemi" }
        };

        if (
            player.Flags.Contains("npc_requested_help") &&
            !_inventoryService.HasItem(player.Id, "forest_herb")
        )
        {
            actions.Add(new()
            {
                Id = "collect_forest_herb",
                Text = "Zbierz rzadkie ziele"
            });
        }
        return actions;
    }

    private ActionResultDto ExplorePaths(Player player)
    {
        if (player.Flags.Contains("cave_discovered"))
        {
            return new ActionResultDto
            {
                Text = "Znasz już te ścieżki. Jedna z nich prowadzi do jaskini."
            };
        }

        player.Flags.Add("cave_discovered");

        return new ActionResultDto
        {
            Text =
                "Przedzierasz się przez zarośla, podążając za ledwo widoczną ścieżką.\n\n" +
                "Po chwili docierasz do skalnego zbocza.\n" +
                "Między korzeniami drzew widzisz ciemny otwór prowadzący w głąb góry.",
            DiscoveredLocations = { "cave" }
        };
    }

    private ActionResultDto LookAround(Player player)
    {
        if (player.Flags.Contains("clearing_house_discovered"))
        {
            return new ActionResultDto
            {
                Text = "Wiesz już, że niedaleko znajduje się polana z domem."
            };
        }

        player.Flags.Add("clearing_house_discovered");

        return new ActionResultDto
        {
            Text =
                "Rozglądasz się uważnie.\n\n" +
                "Między drzewami dostrzegasz jaśniejszy obszar.\n" +
                "Po chwili widzisz niewielką polanę i stojący na niej drewniany dom.",
            DiscoveredLocations = { "clearing_house" }
        };
    }

    private ActionResultDto InspectTracks(Player player)
    {
        if (player.Flags.Contains("forest_tracks_checked"))
        {
            return new ActionResultDto
            {
                Text = "Ślady są stare, ale masz wrażenie, że ktoś nadal krąży po lesie."
            };
        }

        player.Flags.Add("forest_tracks_checked");

        return new ActionResultDto
        {
            Text =
                "Przyglądasz się ziemi.\n\n" +
                "Widzisz ślady butów. Nie są świeże, ale nie należą do ciebie.\n" +
                "Las nie jest tak pusty, jak się wydawało."
        };
    }

    private ActionResultDto CollectForestHerb(Player player)
    {
        if (_inventoryService.HasItem(player.Id, "forest_herb"))
        {
            return new ActionResultDto
            {
                Text = "Masz już to ziele. Nie potrzebujesz więcej."
            };
        }

        var herb = _context.Items.First(i => i.Code == "forest_herb");
        _inventoryService.AddItemToInventory(player.Id, herb.Id)
            .GetAwaiter().GetResult();

        return new ActionResultDto
        {
            Text =
                "Zapuszczasz się głębiej w las.\n\n" +
                "Po dłuższych poszukiwaniach znajdujesz niewielką roślinę\n" +
                "o charakterystycznym zapachu.\n\n" +
                "To musi być to, o czym mówił mieszkaniec polany.",
            Items =
            {
                new()
                {
                    Code = herb.Code,
                    Name = herb.Name,
                    Icon = herb.Icon
                }
            }
        };
    }
}
