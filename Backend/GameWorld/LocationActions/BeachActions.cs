using Backend.Data;
using Backend.Models.Game;
using Backend.DTOs.Game;
using Backend.GameWorld.Quests;
using Backend.Services;

namespace Backend.GameWorld.LocationActions;

public class BeachAction
{
    private readonly GameDbContext _context;
    private readonly QuestService _questService;

    public BeachAction(GameDbContext context, QuestService questService)
    {
        _context = context;
        _questService = questService;
    }

    public ActionResultDto Execute(Player player, string actionId)
    {
        return actionId switch
        {
            "look_around" => LookAround(player),
            "check_shore" => CheckShore(player),
            "search_beach" => SearchBeach(player),
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
            new() { Id = "look_around", Text = "Rozejrzyj się" },
            new() { Id = "check_shore", Text = "Sprawdź brzeg" }
        };

        if (!player.HasFlag("beach_searched"))
        {
            actions.Add(new LocationAction
            {
                Id = "search_beach",
                Text = "Przeszukaj plażę"
            });
        }

        return actions;
    }

    private ActionResultDto LookAround(Player player)
    {
        var result = new ActionResultDto();

        _questService.EnsureQuest(player, QuestIds.EscapeIsland);

        var wreckDiscovered = player.HasFlag("shipwreck_discovered");
        var forestDiscovered = player.HasFlag("forest_discovered");

        if (!wreckDiscovered)
        {
            player.AddFlag("shipwreck_discovered");
            player.AddFlag("forest_discovered");

            _questService.AdvanceQuest(
                player,
                QuestIds.EscapeIsland,
                1,
                QuestDefinitions.EscapeIslandStage
            );

            result.Text =
                "Rozglądasz się uważnie.\n\n" +
                "Niedaleko, przy linii wody, dostrzegasz rozbity statek. " +
                "To jedyne miejsce, gdzie możesz znaleźć cokolwiek użytecznego.\n\n" +
                "Dalej, w głębi wyspy, zaczyna się gęsty las, ale instynkt podpowiada ci, " +
                "że najpierw powinnaś sprawdzić wrak.";

            result.DiscoveredLocations.Add("shipwreck");
            result.DiscoveredLocations.Add("forest_edge");

            return result;
        }

        if (!forestDiscovered)
        {
            player.AddFlag("forest_discovered");

            result.Text =
                "Zauważasz wąską ścieżkę prowadzącą w głąb lasu.\n\n" +
                "Możesz tam pójść, ale masz wrażenie, że wrak statku wciąż skrywa odpowiedzi.";

            result.DiscoveredLocations.Add("forest_edge");

            return result;
        }

        result.Text =
            "Plaża jest pusta i cicha.\n\n" +
            "Wrak statku leży tam, gdzie go widziałaś. Las czeka w milczeniu.";

        return result;
    }

    private ActionResultDto CheckShore(Player player)
    {
        if (player.HasFlag("shore_checked"))
        {
            return new ActionResultDto
            {
                Text = "Morze jest spokojne. Zbyt spokojne."
            };
        }

        player.AddFlag("shore_checked");

        return new ActionResultDto
        {
            Text =
                "Idziesz wzdłuż linii wody.\n\n" +
                "Nie ma żadnych śladów innych ocalałych. " +
                "Brak ciał jest bardziej niepokojący niż ich obecność."
        };
    }

    private ActionResultDto SearchBeach(Player player)
    {
        player.AddFlag("beach_searched");

        return new ActionResultDto
        {
            Text =
                "Przeszukujesz plażę.\n\n" +
                "Znajdujesz jedynie porozrzucane szczątki i fragmenty wyposażenia.\n\n" +
                "Cokolwiek mogło pomóc w ucieczce, zniknęło razem ze sztormem. " +
                "Jeśli chcesz przetrwać, musisz ruszyć dalej w głąb wyspy."
        };
    }
}
