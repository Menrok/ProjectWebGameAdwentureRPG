using Backend.Models.Game;
using Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class QuestService
{
    private readonly GameDbContext _db;

    public QuestService(GameDbContext db)
    {
        _db = db;
    }

    public void EnsureQuest(Player player, string questId)
    {
        var exists = _db.PlayerQuests
            .Any(pq => pq.PlayerId == player.Id && pq.QuestId == questId);

        if (exists)
            return;

        var quest = _db.Quests
            .AsNoTracking()
            .FirstOrDefault(q => q.Id == questId);

        if (quest == null)
            throw new InvalidOperationException(
                $"Quest definition '{questId}' not found. Did you forget to seed it?"
            );

        var playerQuest = new PlayerQuest
        {
            PlayerId = player.Id,
            QuestId = questId,
            CurrentStage = 0,
            Status = QuestStatus.Active,
            Entries = new List<PlayerQuestEntry>()
        };

        _db.PlayerQuests.Add(playerQuest);
    }

    public void AdvanceQuest(
        Player player,
        string questId,
        int newStage,
        Func<int, string> textResolver)
    {
        EnsureQuest(player, questId);

        var quest = player.Quests.FirstOrDefault(q => q.QuestId == questId);

        if (quest == null)
        {
            return;
        }

        if (newStage <= quest.CurrentStage)
            return;

        quest.CurrentStage = newStage;

        quest.Entries.Add(new PlayerQuestEntry
        {
            PlayerId = player.Id,
            QuestId = questId,
            Stage = newStage,
            Text = textResolver(newStage)
        });
    }
}
