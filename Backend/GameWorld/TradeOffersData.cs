using Backend.Models.Game;

namespace Backend.GameWorld;

public static class TradeOffersData
{
    public static readonly TradeOffer SettlementTrader = new()
    {
        TradeId = "settlement_trader",
        TradeType = TradeType.Settlement,
        Items =
        {
            new TradeOfferItem
            {
                ItemCode = "flashlight_basic",
                Quantity = 1,
                Price = 8
            },
            new TradeOfferItem
            {
                ItemCode = "crowbar_basic",
                Quantity = 1,
                Price = 6
            },
            new TradeOfferItem
            {
                ItemCode = "bandage_basic",
                Quantity = 3,
                Price = 2
            }
        }
    };
}
