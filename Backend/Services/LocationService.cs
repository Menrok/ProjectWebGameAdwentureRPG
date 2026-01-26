using Backend.GameWorld;
using Backend.Models.Game;

namespace Backend.Services;

public class LocationService
{
    public Location GetCurrentLocation(Player player)
    {
        return LocationsData.All[player.CurrentLocationId];
    }

    public List<Location> GetConnectedLocations(Player player)
    {
        var current = GetCurrentLocation(player);

        return current.ConnectedLocationIds
            .Select(id => LocationsData.All[id])
            .ToList();
    }

    public void MovePlayer(Player player, string targetLocationId)
    {
        var current = GetCurrentLocation(player);

        if (!current.ConnectedLocationIds.Contains(targetLocationId))
            throw new InvalidOperationException("Nie można przejść do tej lokacji.");

        player.CurrentLocationId = targetLocationId;
    }
}
