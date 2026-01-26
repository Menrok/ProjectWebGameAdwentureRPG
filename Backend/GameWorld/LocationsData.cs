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
                "Ścieżka prowadzi dalej w głąb wyspy.",
            ConnectedLocationIds = new()
            {
                "beach",
                "village"
            }
        },

        ["village"] = new Location
        {
            Id = "village",
            Name = "Wioska",
            Description =
                "Niewielka osada ukryta między drzewami.\n" +
                "Kilka domów i ogniska.\n" +
                "Mieszkańcy patrzą na ciebie nieufnie.",
            ConnectedLocationIds = new()
            {
                "forest"
            }
        }
    };
}
