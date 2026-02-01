namespace Backend.Models.Game;

public class TradeOffer
{
    public string TradeId { get; set; } = "";

    public TradeType TradeType { get; set; }

    public List<TradeOfferItem> Items { get; set; } = new();
}
