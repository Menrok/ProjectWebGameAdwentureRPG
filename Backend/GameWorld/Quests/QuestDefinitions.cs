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

    public static (int stage, string description) EscapeIslandStage(int stage)
        => stage switch
        {
            1 => (1, "Wyspa nie jest pusta. Ktoś tu był — albo nadal jest."),
            2 => (2, "Ktoś próbował tu przetrwać. Obóz może skrywać odpowiedzi."),
            3 => (3, "Notatka wspomina o górze i ukrytym wejściu."),
            4 => (4, "W osadzie żyją inni ludzie. Może wiedzą więcej."),
            _ => (0, "Musisz znaleźć sposób, by opuścić wyspę.")
        };

    public static (int stage, string description) CaveStage(int stage)
        => stage switch
        {
            1 => (1, "Wejście do jaskini zostało zaznaczone na mapie."),
            2 => (2, "Bez światła wejście do jaskini jest zbyt niebezpieczne."),
            3 => (3, "Masz źródło światła. Możesz zbadać jaskinię."),
            4 => (4, "To, co znajduje się w jaskini, zmienia wszystko."),
            _ => (0, "Musisz dowiedzieć się, czym jest jaskinia.")
        };

    public static (int stage, string description) SettlementStage(int stage)
        => stage switch
        {
            1 => (1, "Handlarz wie więcej, niż mówi."),
            2 => (2, "W osadzie wszystko ma swoją cenę."),
            3 => (3, "Każdy tutaj coś ukrywa."),
            _ => (0, "Musisz dowiedzieć się, czym naprawdę jest osada.")
        };
}
