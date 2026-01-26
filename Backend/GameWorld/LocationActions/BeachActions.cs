using Backend.Models.Game;

namespace Backend.GameWorld.LocationActions;

public static class BeachActions
{
    public static List<LocationAction> GetActions(Player player)
    {
        var actions = new List<LocationAction>();

        actions.Add(new LocationAction
        {
            Id = "search_beach",
            Text = "Przeszukaj plażę"
        });

        actions.Add(new LocationAction
        {
            Id = "rest_on_beach",
            Text = "Usiądź i odpocznij"
        });

        return actions;
    }
}
