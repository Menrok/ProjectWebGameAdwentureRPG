namespace Backend.Models.Game;

public class PlayerQuest
{
    public int PlayerId { get; set; }
    public Player Player { get; set; } = null!;

    public string QuestId { get; set; } = "";
    public Quest Quest { get; set; } = null!;

    public int Stage { get; set; }
    public string Description { get; set; } = "";
    public QuestStatus Status { get; set; }
}
