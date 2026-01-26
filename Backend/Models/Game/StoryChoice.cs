namespace Backend.Models.Game;

public class StoryChoice
{
    public string? Id { get; set; }
    public string Text { get; set; } = null!;
    public string? NextNodeId { get; set; }
    public string? ResponseText { get; set; }}
