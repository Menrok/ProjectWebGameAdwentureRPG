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

        return current.ConnectedLocationIds.Select(id => LocationsData.All[id]).Where(loc => loc.RequiredFlags.All(player.HasFlag)).ToList();
    }

    public void MovePlayer(Player player, string targetLocationId)
    {
        var available = GetConnectedLocations(player);

        if (!available.Any(l => l.Id == targetLocationId))
            throw new InvalidOperationException("Nie możesz tam teraz pójść.");

        player.CurrentLocationId = targetLocationId;
    }
}
