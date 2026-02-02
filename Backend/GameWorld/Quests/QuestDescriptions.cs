namespace Backend.GameWorld.Quests;

public static class QuestDescriptions
{
    public static string GetInitialDescription(string questId) => questId switch
    {
        QuestIds.EscapeIsland =>
            "Jesteś uwięziona na nieznanej wyspie. Musisz znaleźć sposób, by się stąd wydostać.",

        QuestIds.CaveMystery =>
            "W notatce wspomniano o wejściu w górach. Musisz dowiedzieć się, co się tam znajduje.",

        QuestIds.Settlement =>
            "Na wyspie istnieje osada. Ludzie tu żyją, ale sprawiają wrażenie, jakby czekali.",

        _ => ""
    };
}
