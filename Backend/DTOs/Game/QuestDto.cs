using Backend.Models.Game;

namespace Backend.DTOs.Game;

public class QuestDto
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public int Stage { get; set; }
    public QuestStatus Status { get; set; }
}
