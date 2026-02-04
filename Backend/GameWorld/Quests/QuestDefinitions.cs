using Backend.Models.Game;

namespace Backend.GameWorld.Quests;

public static class QuestDefinitions
{
    public static Quest EscapeIsland => new()
    {
        Id = QuestIds.EscapeIsland,
        Title = "Wydostać się z wyspy"
    };

    public static Quest CaveMystery => new()
    {
        Id = QuestIds.CaveMystery,
        Title = "Jaskinia"
    };

    public static Quest Settlement => new()
    {
        Id = QuestIds.Settlement,
        Title = "Osada"
    };

    public static string EscapeIslandStage(int stage) => stage switch
    {
        1 => "Wyspa nie jest pusta. Ktoś tu był — albo nadal jest.",
        2 => "Ktoś próbował tu przetrwać. Opuszczony obóz może skrywać odpowiedzi.",
        3 => "Notatka wspomina o górze i ukrytym wejściu.",
        4 => "W osadzie żyją inni ludzie. Może wiedzą więcej.",
        _ => "Musisz znaleźć sposób, by opuścić wyspę."
    };

    public static string CaveStage(int stage) => stage switch
    {
        1 => "Wejście do jaskini zostało zaznaczone na mapie.",
        2 => "Bez światła wejście do jaskini jest zbyt niebezpieczne.",
        3 => "Masz źródło światła. Możesz zbadać jaskinię.",
        4 => "To, co znajduje się w jaskini, zmienia wszystko.",
        _ => "Musisz dowiedzieć się, czym naprawdę jest jaskinia."
    };

    public static string SettlementStage(int stage) => stage switch
    {
        1 => "Handlarz wie więcej, niż mówi.",
        2 => "W osadzie wszystko ma swoją cenę.",
        3 => "Każdy tutaj coś ukrywa.",
        _ => "Musisz dowiedzieć się, czym naprawdę jest osada."
    };
}
