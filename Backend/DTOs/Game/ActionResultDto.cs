namespace Backend.DTOs.Game;

public class ActionResultDto
{
    public string Text { get; set; } = "";

    public int? HpChange { get; set; }
    public int? CrystalChange { get; set; }

    public List<ItemRewardDto> Items { get; set; } = new();
    public List<string> DiscoveredLocations { get; set; } = new();

    public string? OpenTradeId { get; set; }
}
