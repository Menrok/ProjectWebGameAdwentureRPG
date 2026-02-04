namespace Backend.Models.Game;

public class PlayerQuestEntry
{
    public int Id { get; set; }

    public int PlayerId { get; set; }
    public Player Player { get; set; } = null!;

    public string QuestId { get; set; } = "";
    public Quest Quest { get; set; } = null!;

    public int Stage { get; set; }
    public string Text { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}