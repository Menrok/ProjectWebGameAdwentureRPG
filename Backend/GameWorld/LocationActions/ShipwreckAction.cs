using Backend.Data;
using Backend.Models.Game;
using Backend.DTOs.Game;
using Backend.Services;
using Backend.GameWorld.Quests;

namespace Backend.GameWorld.LocationActions;

public class ShipwreckAction
{
    private readonly GameDbContext _context;
    private readonly InventoryService _inventoryService;
    private readonly QuestService _questService;

    public ShipwreckAction(
        GameDbContext context,
        InventoryService inventoryService,
        QuestService questService)
    {
        _context = context;
        _inventoryService = inventoryService;
        _questService = questService;
    }

    public ActionResultDto Execute(Player player, string actionId)
    {
        return actionId switch
        {
            "enter_wreck" => EnterWreck(player),
            "search_cabins" => SearchCabins(player),
            "check_radio" => CheckRadio(player),
            "check_cargo" => CheckCargo(player),
            "locked_cabin" => LockedCabin(player),
            "check_medkit" => CheckMedkit(player),
            _ => new ActionResultDto { Text = "Nie możesz tego teraz zrobić." }
        };
    }

    public List<LocationAction> GetAvailableActions(Player player)
    {
        var actions = new List<LocationAction>
        {
            new() { Id = "enter_wreck", Text = "Wejdź do wraku" },
            new() { Id = "check_radio", Text = "Sprawdź radio" },
            new() { Id = "check_cargo", Text = "Sprawdź ładownię" }
        };

        if (!player.HasFlag("cabins_searched"))
        {
            actions.Add(new LocationAction
            {
                Id = "search_cabins",
                Text = "Przeszukaj kajuty"
            });
        }

        if (!player.HasFlag("locked_cabin_found"))
        {
            actions.Add(new LocationAction
            {
                Id = "locked_cabin",
                Text = "Zamknięta kabina"
            });
        }

        if (!player.HasFlag("medkit_looted"))
        {
            actions.Add(new LocationAction
            {
                Id = "check_medkit",
                Text = "Sprawdź apteczkę"
            });
        }

        return actions;
    }

    private ActionResultDto EnterWreck(Player player)
    {
        if (player.HasFlag("wreck_entered"))
        {
            return new ActionResultDto
            {
                Text = "Wrak wygląda dokładnie tak samo. Martwy i cichy."
            };
        }

        player.AddFlag("wreck_entered");

        _questService.AdvanceQuest(
            player,
            QuestIds.EscapeIsland,
            1,
            QuestDefinitions.EscapeIslandStage
        );

        return new ActionResultDto
        {
            Text =
                "Wchodzisz do wnętrza wraku.\n\n" +
                "Zapach soli i paliwa unosi się w powietrzu. " +
                "Statek jest zniszczony, ale wiele elementów wygląda na naruszone już po katastrofie."
        };
    }

    private ActionResultDto SearchCabins(Player player)
    {
        player.AddFlag("cabins_searched");

        if (!player.HasFlag("backpack_obtained"))
        {
            var backpack = _context.Items.First(i => i.Code == "backpack_basic");
            var knife = _context.Items.First(i => i.Code == "knife_basic");

            _inventoryService.AddItemToInventory(player.Id, backpack.Id).Wait();
            _inventoryService.AddItemToInventory(player.Id, knife.Id).Wait();

            player.AddFlag("backpack_obtained");

            _questService.AdvanceQuest(
                player,
                QuestIds.EscapeIsland,
                1,
                QuestDefinitions.EscapeIslandStage
            );

            return new ActionResultDto
            {
                Text =
                    "Przeszukujesz kajuty.\n\n" +
                    "W jednej z nich znajdujesz plecak. W środku ktoś zostawił nóż — " +
                    "prosty, ale wciąż użyteczny.\n\n" +
                    "Od teraz nie jesteś już całkiem bezbronna.",
                Items =
                {
                    new() { Code = backpack.Code, Name = backpack.Name, Icon = backpack.Icon },
                    new() { Code = knife.Code, Name = knife.Name, Icon = knife.Icon }
                }
            };
        }

        return new ActionResultDto
        {
            Text =
                "Przeszukujesz kajuty ponownie.\n\n" +
                "Nic nowego. Wszystko, co miało znaczenie, już znalazłaś."
        };
    }

    private ActionResultDto CheckMedkit(Player player)
    {
        player.AddFlag("medkit_looted");

        var bandage = _context.Items.First(i => i.Code == "bandage_basic");

        _inventoryService
            .AddItemToInventory(player.Id, bandage.Id)
            .GetAwaiter()
            .GetResult();

        return new ActionResultDto
        {
            Text =
                "Otwierasz apteczkę pokładową.\n\n" +
                "Jest prawie pusta. Został tylko jeden bandaż. " +
                "Ktoś musiał zabrać resztę w pośpiechu.",
            Items =
            {
                new()
                {
                    Code = bandage.Code,
                    Name = bandage.Name,
                    Icon = bandage.Icon
                }
            }
        };
    }

    private ActionResultDto CheckRadio(Player player)
    {
        if (player.HasFlag("radio_checked"))
        {
            return new ActionResultDto
            {
                Text = "Radio nadal nie działa. Cisza."
            };
        }

        player.AddFlag("radio_checked");

        return new ActionResultDto
        {
            Text =
                "Sprawdzasz radio.\n\n" +
                "Przez chwilę słychać szum, jakby ktoś próbował się przebić… " +
                "Po chwili wszystko cichnie."
        };
    }

    private ActionResultDto CheckCargo(Player player)
    {
        if (player.HasFlag("cargo_checked"))
        {
            return new ActionResultDto
            {
                Text = "Ładownia jest pusta. Nic się nie zmieniło."
            };
        }

        player.AddFlag("cargo_checked");

        _questService.AdvanceQuest(
            player,
            QuestIds.EscapeIsland,
            2,
            QuestDefinitions.EscapeIslandStage
        );

        return new ActionResultDto
        {
            Text =
                "Zaglądasz do ładowni.\n\n" +
                "Jest niemal pusta. To nie wygląda na efekt sztormu.\n\n" +
                "Ktoś musiał wynieść zawartość celowo."
        };
    }

    private ActionResultDto LockedCabin(Player player)
    {
        player.AddFlag("locked_cabin_found");

        return new ActionResultDto
        {
            Text =
                "Jedna z kabin jest zamknięta.\n\n" +
                "Drzwi zostały zabezpieczone od zewnątrz. " +
                "Bez odpowiedniego narzędzia nie masz szans ich otworzyć."
        };
    }
}
