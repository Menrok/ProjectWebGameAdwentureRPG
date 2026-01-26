namespace Backend.Models.Game;

public class StoryNode
{
    public string Id { get; set; } = null!;
    public string Text { get; set; } = null!;
    public List<StoryChoice> Choices { get; set; } = new();
    public bool IsFreeMode { get; set; } = false;
    public bool IsDialog { get; set; } = false;
}
