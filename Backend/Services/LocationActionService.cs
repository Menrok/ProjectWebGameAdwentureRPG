using Backend.Models.Game;

namespace Backend.Services;

public class LocationActionService
{
    public string Execute(Player player, string actionId)
    {
        return player.CurrentLocationId switch
        {
            "beach" => ExecuteBeachAction(player, actionId),
            _ => "Nic się nie dzieje."
        };
    }

    private string ExecuteBeachAction(Player player, string actionId)
    {
        switch (actionId)
        {
            case "search_beach":
                if (player.Flags.Contains("beach_searched"))
                    return "Przeszukałaś już plażę. Nie znajdujesz niczego nowego.";

                player.Flags.Add("beach_searched");

                return
                    "Przeszukujesz plażę.\n\n" +
                    "Wśród mokrego piasku znajdujesz fragment metalu z wraku statku.\n" +
                    "Może się jeszcze przydać.";

            case "rest_on_beach":
                return
                    "Siadasz na piasku i pozwalasz sobie na chwilę odpoczynku.\n\n" +
                    "Ból powoli ustępuje, a myśli się uspokajają.";

            default:
                return "Nie możesz tego teraz zrobić.";
        }
    }

    public List<LocationAction> GetAvailableActions(Player player)
    {
        if (player.CurrentLocationId == "beach")
        {
            return new List<LocationAction>
            {
                new() { Id = "search_beach", Text = "Przeszukaj plażę" },
                new() { Id = "rest_on_beach", Text = "Usiądź i odpocznij" }
            };
        }

        return new List<LocationAction>();
    }
}
