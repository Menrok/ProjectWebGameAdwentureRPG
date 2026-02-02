using Backend.GameWorld.Quests;
using Backend.Models.Game;

namespace Backend.Services;

public class QuestService
{
    public void EnsureQuest(Player player, string questId)
    {
        if (player.Quests.Any(q => q.QuestId == questId))
            return;

        var quest = new Quest
        {
            Id = questId,
            Title = QuestTitles.GetTitle(questId)
        };

        var playerQuest = new PlayerQuest
        {
            QuestId = quest.Id,
            Quest = quest,
            Stage = 0,
            Description = QuestDescriptions.GetInitialDescription(questId),
            Status = QuestStatus.Active
        };

        player.Quests.Add(playerQuest);
    }

    public void AdvanceQuest(
        Player player,
        string questId,
        int newStage,
        Func<int, (int stage, string description)> resolver)
    {
        var pq = player.Quests.First(q => q.QuestId == questId);

        if (newStage <= pq.Stage)
            return;

        var (stage, description) = resolver(newStage);

        pq.Stage = stage;
        pq.Description = description;
    }
}
