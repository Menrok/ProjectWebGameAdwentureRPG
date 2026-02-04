using Backend.Data;
using Backend.Models.Game;
using Backend.DTOs.Game;
using Backend.Services;
using Backend.GameWorld.Quests;

namespace Backend.GameWorld.LocationActions;

public class AbandonedCampAction
{
    private readonly GameDbContext _context;
    private readonly QuestService _questService;

    public AbandonedCampAction(GameDbContext context, QuestService questService)
    {
        _context = context;
        _questService = questService;
    }

    public ActionResultDto Execute(Player player, string actionId)
    {
        return actionId switch
        {
            "enter_camp" => EnterCamp(player),
            "search_camp" => SearchCamp(player),
            "read_note" => ReadNote(player),
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
            new() { Id = "enter_camp", Text = "Podejdź do obozu" }
        };

        if (!player.HasFlag("camp_searched"))
        {
            actions.Add(new LocationAction
            {
                Id = "search_camp",
                Text = "Przeszukaj obóz"
            });
        }

        if (player.HasFlag("camp_searched") && !player.HasFlag("camp_note_read"))
        {
            actions.Add(new LocationAction
            {
                Id = "read_note",
                Text = "Przeczytaj znalezioną notatkę"
            });
        }

        return actions;
    }

    private ActionResultDto EnterCamp(Player player)
    {
        if (player.HasFlag("camp_entered"))
        {
            return new ActionResultDto
            {
                Text =
                    "Obóz wygląda dokładnie tak samo.\n\n" +
                    "Porzucony w pośpiechu. Jakby ktoś nie planował tu wracać."
            };
        }

        player.AddFlag("camp_entered");

        _questService.AdvanceQuest(
            player,
            QuestIds.EscapeIsland,
            2,
            QuestDefinitions.EscapeIslandStage
        );

        return new ActionResultDto
        {
            Text =
                "Docierasz do niewielkiego obozu.\n\n" +
                "Wygaszone ognisko, prowizoryczne posłanie i kilka porozrzucanych przedmiotów.\n" +
                "Ktoś był tu niedawno."
        };
    }

    private ActionResultDto SearchCamp(Player player)
    {
        player.AddFlag("camp_searched");

        player.Crystals += 2;

        return new ActionResultDto
        {
            Text =
                "Przeszukujesz obóz.\n\n" +
                "Znajdujesz fragment mapy wyspy. Ktoś zaznaczył na niej miejsce u podnóża góry.\n\n" +
                "Wśród porzuconych rzeczy leżą dwa matowe kryształy. " +
                "Wyglądają na cenne — i na takie, których nikt tu nie zostawia przypadkiem.",
            CrystalChange = 2
        };
    }

    private ActionResultDto ReadNote(Player player)
    {
        if (player.HasFlag("camp_note_read"))
        {
            return new ActionResultDto
            {
                Text =
                    "Czytasz notatkę jeszcze raz.\n\n" +
                    "Bez światła wejście do jaskini to samobójstwo."
            };
        }

        player.AddFlag("camp_note_read");
        player.AddFlag("settlement_discovered");
        player.AddFlag("cave_discovered");

        _questService.AdvanceQuest(
            player,
            QuestIds.CaveMystery,
            1,
            QuestDefinitions.CaveStage
        );

        _questService.AdvanceQuest(
            player,
            QuestIds.EscapeIsland,
            3,
            QuestDefinitions.EscapeIslandStage
        );

        return new ActionResultDto
        {
            Text =
                "Rozwijasz kartkę.\n\n" +
                "\"Wejście jest tam, gdzie skała pęka.\n" +
                "Bez światła nie ma sensu próbować.\n" +
                "Osada na północy… Może tam znajdę to, czego potrzebuję.\"",
            DiscoveredLocations = { "settlement" }
        };
    }
}
