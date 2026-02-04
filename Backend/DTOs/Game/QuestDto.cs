using Backend.Models.Game;

namespace Backend.DTOs.Game;

public class QuestDto
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public int CurrentStage { get; set; }
    public string Status { get; set; } = "";

    public List<QuestEntryDto> Entries { get; set; } = new();
}
