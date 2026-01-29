using Backend.GameWorld.LocationActions;
using Backend.Models.Game;
using Backend.DTOs.Game;

namespace Backend.Services;

public class LocationActionService
{
    private readonly BeachAction _beachAction;
    private readonly ForestAction _forestAction;
    private readonly ClearingHouseAction _clearingHouseAction;

    public LocationActionService( BeachAction beachAction, ForestAction forestAction, ClearingHouseAction clearingHouseAction)
    {
        _beachAction = beachAction;
        _forestAction = forestAction;
        _clearingHouseAction = clearingHouseAction;
    }

    public ActionResultDto Execute(Player player, string actionId)
    {
        return player.CurrentLocationId switch
        {
            "beach" => _beachAction.Execute(player, actionId),
            "forest" => _forestAction.Execute(player, actionId),
            "clearing_house" => _clearingHouseAction.Execute(player, actionId),
            _ => new ActionResultDto
            {
                Text = "Nie możesz tego teraz zrobić."
            }
        };
    }

    public List<LocationAction> GetAvailableActions(Player player)
    {
        return player.CurrentLocationId switch
        {
            "beach" => _beachAction.GetAvailableActions(player),
            "forest" => _forestAction.GetAvailableActions(player),
            "clearing_house" => _clearingHouseAction.GetAvailableActions(player),
            _ => new()
        };
    }
}
