using Backend.GameWorld.LocationActions;
using Backend.Models.Game;
using Backend.DTOs.Game;

namespace Backend.Services;

public class LocationActionService
{
    private readonly BeachAction _beachAction;

    public LocationActionService(BeachAction beachAction)
    {
        _beachAction = beachAction;
    }

    public ActionResultDto Execute(Player player, string actionId)
    {
        return player.CurrentLocationId switch
        {
            "beach" => _beachAction.Execute(player, actionId),
            _ => new ActionResultDto { Text = "Nic się nie dzieje." }
        };
    }

    public List<LocationAction> GetAvailableActions(Player player)
    {
        return player.CurrentLocationId switch
        {
            "beach" => _beachAction.GetAvailableActions(player),
            _ => new()
        };
    }
}
