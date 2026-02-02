namespace Backend.DTOs.Game;

public class TradeItemDto
{
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Icon { get; set; } = "";
    public int Price { get; set; }
    public int Quantity { get; set; }
}
