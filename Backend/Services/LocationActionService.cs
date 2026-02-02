using Backend.GameWorld.LocationActions;
using Backend.Models.Game;
using Backend.DTOs.Game;

namespace Backend.Services;

public class LocationActionService
{
    private readonly Dictionary<string, Func<Player, string, ActionResultDto>> _executeMap;
    private readonly Dictionary<string, Func<Player, List<LocationAction>>> _actionsMap;

    public LocationActionService(
        BeachAction beach,
        ForestAction forest,
        ShipwreckAction shipwreck,
        AbandonedCampAction camp,
        CaveEntranceAction cave,
        SettlementAction settlement)
    {
        _executeMap = new()
        {
            ["beach"] = beach.Execute,
            ["forest"] = forest.Execute,
            ["shipwreck"] = shipwreck.Execute,
            ["abandoned_camp"] = camp.Execute,
            ["cave"] = cave.Execute,
            ["village"] = settlement.Execute
        };

        _actionsMap = new()
        {
            ["beach"] = beach.GetAvailableActions,
            ["forest"] = forest.GetAvailableActions,
            ["shipwreck"] = shipwreck.GetAvailableActions,
            ["abandoned_camp"] = camp.GetAvailableActions,
            ["cave"] = cave.GetAvailableActions,
            ["village"] = settlement.GetAvailableActions
        };
    }

    public ActionResultDto Execute(Player player, string actionId)
    {
        if (!_executeMap.TryGetValue(player.CurrentLocationId, out var handler))
        {
            return new ActionResultDto
            {
                Text = "Nie możesz tego teraz zrobić."
            };
        }

        return handler(player, actionId);
    }

    public List<LocationAction> GetAvailableActions(Player player)
    {
        if (!_actionsMap.TryGetValue(player.CurrentLocationId, out var handler))
            return new();

        return handler(player);
    }
}
