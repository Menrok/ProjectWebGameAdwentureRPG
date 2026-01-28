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

        var connected = current.ConnectedLocationIds
            .Select(id => LocationsData.All[id])
            .ToList();

        if (player.CurrentLocationId == "beach" &&
            !player.Flags.Contains("forest_discovered"))
        {
            connected.RemoveAll(l => l.Id == "forest");
        }

        return connected;
    }

    public void MovePlayer(Player player, string targetLocationId)
    {
        var current = GetCurrentLocation(player);

        if (!current.ConnectedLocationIds.Contains(targetLocationId))
            throw new InvalidOperationException("Nie można przejść do tej lokacji.");

        if (current.Id == "beach" &&
            targetLocationId == "forest" &&
            !player.Flags.Contains("forest_discovered"))
        {
            throw new InvalidOperationException(
                "Nie wiesz jeszcze, dokąd prowadzi ta droga."
            );
        }

        player.CurrentLocationId = targetLocationId;
    }
}
