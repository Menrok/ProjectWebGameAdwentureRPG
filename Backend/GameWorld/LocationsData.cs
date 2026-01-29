using Backend.Models.Game;

namespace Backend.GameWorld;

public static class LocationsData
{
    public static readonly Dictionary<string, Location> All = new()
    {
        ["beach"] = new Location
        {
            Id = "beach",
            Name = "Plaża",
            Description =
                "Rozległa, kamienista plaża.\n" +
                "Na piasku wciąż widać ślady sztormu.\n" +
                "Morze jest spokojne, ale niepokojąco ciche.",
            ConnectedLocationIds = new()
            {
                "forest"
            }
        },

        ["forest"] = new Location
        {
            Id = "forest",
            Name = "Las",
            Description =
                "Gęsty, wilgotny las.\n" +
                "Światło ledwo przebija się przez korony drzew.\n" +
                "Między drzewami widać ślady dawnych ścieżek.",
            ConnectedLocationIds = new()
            {
                "beach",
                "clearing_house",
                "cave"
            }
        },

        ["cave"] = new Location
        {
            Id = "cave",
            Name = "Jaskinia",
            Description =
                "Wejście do jaskini ukryte jest między skałami.\n" +
                "Z wnętrza dobiega chłodny podmuch powietrza.\n" +
                "Ciemność w środku zdaje się pochłaniać światło.",
            ConnectedLocationIds = new()
            {
                "forest"
            }
        },

        ["clearing_house"] = new Location
        {
            Id = "clearing_house",
            Name = "Dom na polanie",
            Description =
                "Drewniany dom stoi na niewielkiej polanie, otoczony lasem.\n" +
                "W oknach tli się słabe światło, a drzwi nie są zamknięte.\n" +
                "Ktoś tu mieszka — i najwyraźniej nie śpi.",
            ConnectedLocationIds = new()
            {
                "forest",
                "village"
            }
        },

        ["village"] = new Location
        {
            Id = "village",
            Name = "Wioska",
            Description =
                "Niewielka osada przy rzece.\n" +
                "Kilka drewnianych domów i tlące się ogniska.\n",
            ConnectedLocationIds = new()
            {
                "clearing_house"
            }
        }
    };
}
