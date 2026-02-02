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
                "Na piasku wciąż widać ślady niedawnego sztormu.\n" +
                "Morze jest spokojne, ale niepokojąco ciche.",
            ConnectedLocationIds = { "shipwreck", "forest" },


        },

        ["shipwreck"] = new Location
        {
            Id = "shipwreck",
            Name = "Wrak statku",
            Description =
                "Rozbity statek leży częściowo zanurzony w wodzie.\n" +
                "Kadłub jest przełamany, a wnętrze wygląda na naruszone już po katastrofie.\n" +
                "To jedyne miejsce, gdzie możesz znaleźć coś użytecznego.",
            ConnectedLocationIds = { "beach" },
            RequiredFlags = { "shipwreck_discovered" }
        },

        ["forest"] = new Location
        {
            Id = "forest",
            Name = "Las",
            Description =
                "Gęsty, wilgotny las.\n" +
                "Światło ledwo przebija się przez korony drzew.\n" +
                "Między drzewami widać ślady dawnych ścieżek.",
            ConnectedLocationIds = { "beach", "abandoned_camp" },
            RequiredFlags = { "forest_discovered" }
        },

        ["abandoned_camp"] = new Location
        {
            Id = "abandoned_camp",
            Name = "Opuszczony obóz",
            Description =
                "Prowizoryczny obóz na niewielkiej polanie.\n" +
                "Wygaszone ognisko i porzucone wyposażenie.\n" +
                "Ktoś był tu niedawno — i ruszył dalej.",
            ConnectedLocationIds = { "forest", "cave", "village" },
            RequiredFlags = { "camp_discovered" }
        },

        ["cave"] = new Location
        {
            Id = "cave",
            Name = "Jaskinia",
            Description =
                "Wejście do jaskini ukryte jest między skałami u podnóża góry.\n" +
                "Z wnętrza dobiega chłodny podmuch powietrza.\n" +
                "Ciemność w środku zdaje się pochłaniać światło.",
            ConnectedLocationIds = { "abandoned_camp" },
            RequiredFlags = { "cave_discovered" }
        },

        ["village"] = new Location
        {
            Id = "village",
            Name = "Osada",
            Description =
                "Niewielka osada na skraju lasu.\n" +
                "Kilka drewnianych baraków i tlące się ogniska.\n" +
                "To jedyne miejsce, gdzie można zdobyć potrzebny sprzęt.",
            ConnectedLocationIds = { "abandoned_camp" },
            RequiredFlags = { "settlement_discovered" }
        }
    };
}
