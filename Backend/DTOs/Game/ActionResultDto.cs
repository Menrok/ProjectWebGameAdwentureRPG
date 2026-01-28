namespace Backend.DTOs.Game;

public class ActionResultDto
{
    public string Text { get; set; } = "";
    public List<ItemRewardDto> Items { get; set; } = new();
    public List<string> DiscoveredLocations { get; set; } = new();
}