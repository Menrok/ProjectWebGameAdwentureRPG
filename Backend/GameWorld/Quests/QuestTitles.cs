namespace Backend.GameWorld.Quests;

public static class QuestTitles
{
    public static string GetTitle(string questId) => questId switch
    {
        QuestIds.EscapeIsland => "Wydostać się z wyspy",
        QuestIds.CaveMystery => "Jaskinia",
        QuestIds.Settlement => "Osada",
        _ => "Nieznane zadanie"
    };
}
