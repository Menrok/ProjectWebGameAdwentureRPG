using Backend.Data;
using Backend.Models.Game;
using Backend.DTOs.Game;
using Backend.GameWorld.Quests;
using Backend.Services;

namespace Backend.GameWorld.LocationActions;

public class CaveEntranceAction
{
    private readonly GameDbContext _context;
    private readonly QuestService _questService;

    public CaveEntranceAction(GameDbContext context, QuestService questService)
    {
        _context = context;
        _questService = questService;
    }

    public ActionResultDto Execute(Player player, string actionId)
    {
        return actionId switch
        {
            "approach_cave" => ApproachCave(player),
            "inspect_entrance" => InspectEntrance(player),
            "enter_cave" => EnterCave(player),
            _ => new ActionResultDto
            {
                Text = "Nie możesz tego teraz zrobić."
            }
        };
    }

    public List<LocationAction> GetAvailableActions(Player player)
    {
        return new List<LocationAction>
        {
            new() { Id = "approach_cave", Text = "Podejdź do jaskini" },
            new() { Id = "inspect_entrance", Text = "Zbadaj wejście" },
            new() { Id = "enter_cave", Text = "Wejdź do jaskini" }
        };
    }

    private ActionResultDto ApproachCave(Player player)
    {
        if (player.HasFlag("cave_entered"))
        {
            return new ActionResultDto
            {
                Text =
                    "Stoisz przed wejściem do jaskini.\n\n" +
                    "Ciemność w środku wydaje się niemal namacalna."
            };
        }

        player.AddFlag("cave_entered");

        _questService.AdvanceQuest(
            player,
            QuestIds.CaveMystery,
            1,
            QuestDefinitions.CaveStage
        );

        return new ActionResultDto
        {
            Text =
                "Docierasz do wejścia jaskini.\n\n" +
                "Znajduje się u podnóża góry. Skała wygląda, jakby została tu naruszona " +
                "w sposób nienaturalny."
        };
    }

    private ActionResultDto InspectEntrance(Player player)
    {
        if (!player.HasFlag("cave_inspected"))
        {
            player.AddFlag("cave_inspected");

            _questService.AdvanceQuest(
                player,
                QuestIds.CaveMystery,
                2,
                QuestDefinitions.CaveStage
            );
        }

        return new ActionResultDto
        {
            Text =
                "Przyglądasz się uważnie wejściu.\n\n" +
                "Widać ślady częstego użytkowania. Ktoś wchodził i wychodził stąd wielokrotnie.\n\n" +
                "Bez odpowiedniego światła dalsza droga byłaby czystą głupotą."
        };
    }

    private ActionResultDto EnterCave(Player player)
    {
        var hasFlashlight = player.Inventory
            .Any(i => i.Item.Code == "flashlight_basic");

        if (!hasFlashlight)
        {
            player.AddFlag("cave_blocked_darkness");

            return new ActionResultDto
            {
                Text =
                    "Robisz kilka kroków w głąb jaskini.\n\n" +
                    "Ciemność natychmiast cię pochłania. Bez źródła światła " +
                    "dalsza droga jest niemożliwa.\n\n" +
                    "To nie jest miejsce na improwizację."
            };
        }

        _questService.AdvanceQuest(
            player,
            QuestIds.CaveMystery,
            3,
            QuestDefinitions.CaveStage
        );

        _questService.AdvanceQuest(
            player,
            QuestIds.EscapeIsland,
            5,
            QuestDefinitions.EscapeIslandStage
        );

        return new ActionResultDto
        {
            Text =
                "Włączasz latarkę.\n\n" +
                "Snop światła rozcina ciemność, odsłaniając wąski korytarz prowadzący w głąb góry.\n\n" +
                "To, co znajduje się dalej, zmieni wszystko."
        };
    }
}
