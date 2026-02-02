using Backend.Data;
using Backend.Models.Game;
using Backend.DTOs.Game;
using Backend.Services;
using Backend.GameWorld.Quests;

namespace Backend.GameWorld.LocationActions;

public class ForestAction
{
    private readonly GameDbContext _context;
    private readonly CombatService _combatService;
    private readonly QuestService _questService;

    public ForestAction(GameDbContext context, CombatService combatService, QuestService questService)
    {
        _context = context;
        _combatService = combatService;
        _questService = questService;
    }

    public ActionResultDto Execute(Player player, string actionId)
    {
        return actionId switch
        {
            "enter_forest" => EnterForest(player),
            "check_signs" => CheckSigns(player),
            "follow_path" => FollowPath(player),
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
            new() { Id = "enter_forest", Text = "Wejdź do lasu" }
        };

        if (!player.HasFlag("forest_signs_checked"))
        {
            actions.Add(new LocationAction
            {
                Id = "check_signs",
                Text = "Zbadaj otoczenie"
            });
        }

        actions.Add(new LocationAction
        {
            Id = "follow_path",
            Text = "Podążaj ścieżką"
        });

        return actions;
    }

    private ActionResultDto EnterForest(Player player)
    {
        if (player.HasFlag("forest_entered"))
        {
            return new ActionResultDto
            {
                Text =
                    "Las jest cichy. Zbyt cichy.\n\n" +
                    "Masz wrażenie, że coś tu nie pasuje."
            };
        }

        player.AddFlag("forest_entered");

        _questService.AdvanceQuest(
            player,
            QuestIds.EscapeIsland,
            1,
            QuestDefinitions.EscapeIslandStage
        );

        return new ActionResultDto
        {
            Text =
                "Wchodzisz między drzewa.\n\n" +
                "Światło szybko znika, a powietrze robi się ciężkie. " +
                "Każdy dźwięk odbija się echem, jakby las nasłuchiwał twoich kroków."
        };
    }

    private ActionResultDto CheckSigns(Player player)
    {
        player.AddFlag("forest_signs_checked");

        return new ActionResultDto
        {
            Text =
                "Przyglądasz się uważnie otoczeniu.\n\n" +
                "Na drzewach widzisz nacięcia. Nie wyglądają na przypadkowe.\n\n" +
                "Ktoś oznaczał drogę w głąb wyspy. I nie robił tego dla zabawy."
        };
    }

    private ActionResultDto FollowPath(Player player)
    {
        if (player.HasFlag("camp_discovered"))
        {
            return new ActionResultDto
            {
                Text = "To ta sama ścieżka. Prowadzi do opuszczonego obozu."
            };
        }

        player.AddFlag("forest_path_taken");
        player.AddFlag("camp_discovered");

        _questService.AdvanceQuest(
            player,
            QuestIds.EscapeIsland,
            2,
            QuestDefinitions.EscapeIslandStage
        );

        if (!player.HasFlag("forest_hazard_triggered"))
        {
            player.AddFlag("forest_hazard_triggered");
            _combatService.ApplyEnvironmentalDamage(player, 5);

            return new ActionResultDto
            {
                Text =
                    "Podążasz wąską ścieżką.\n\n" +
                    "Ziemia pod stopami jest śliska. Na moment tracisz równowagę " +
                    "i boleśnie uderzasz o korzeń.\n\n" +
                    "To miejsce nie wybacza nieuwagi.",
                HpChange = -5,
                DiscoveredLocations = { "abandoned_camp" }
            };
        }

        return new ActionResultDto
        {
            Text =
                "Podążasz oznaczoną ścieżką.\n\n" +
                "Po chwili między drzewami wyłania się opuszczony obóz.",
            DiscoveredLocations = { "abandoned_camp" }
        };
    }
}
