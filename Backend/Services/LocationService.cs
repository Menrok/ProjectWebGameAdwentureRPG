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

        if (player.CurrentLocationId == "forest" &&
            !player.Flags.Contains("clearing_house_discovered"))
        {
            connected.RemoveAll(l => l.Id == "clearing_house");
        }

        if (player.CurrentLocationId == "forest" &&
            !player.Flags.Contains("cave_discovered"))
        {
            connected.RemoveAll(l => l.Id == "cave");
        }
        
        if (player.CurrentLocationId == "clearing_house" &&
            !player.Flags.Contains("village_discovered"))
        {
            connected.RemoveAll(l => l.Id == "village");
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
