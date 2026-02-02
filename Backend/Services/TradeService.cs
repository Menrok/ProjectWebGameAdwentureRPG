using Backend.Data;
using Backend.DTOs.Game;
using Backend.GameWorld;
using Backend.Models.Game;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class TradeService
{
    private readonly GameDbContext _context;
    private readonly InventoryService _inventoryService;

    public TradeService(GameDbContext context, InventoryService inventoryService)
    {
        _context = context;
        _inventoryService = inventoryService;
    }

    public List<TradeItemDto> GetTradeItems(string tradeId)
    {
        var offer = GetOffer(tradeId);

        var itemCodes = offer.Items
            .Select(i => i.ItemCode)
            .ToList();

        var items = _context.Items
            .Where(i => itemCodes.Contains(i.Code))
            .ToList();

        return offer.Items
            .Join(
                items,
                offerItem => offerItem.ItemCode,
                item => item.Code,
                (offerItem, item) => new TradeItemDto
                {
                    Code = item.Code,
                    Name = item.Name,
                    Description = item.Description,
                    Icon = item.Icon,
                    Price = offerItem.Price,
                    Quantity = offerItem.Quantity
                }
            )
            .ToList();
    }

    public async Task<int> BuyItem(int userId, string tradeId, string itemCode)
    {
        var player = await _context.Players.Include(p => p.Flags).FirstAsync(p => p.UserId == userId);

        var offer = GetOffer(tradeId);

        var offerItem = offer.Items.FirstOrDefault(i => i.ItemCode == itemCode);
        if (offerItem == null)
            throw new InvalidOperationException("Ten przedmiot nie jest dostępny u handlarza.");

        if (offerItem.Quantity <= 0)
            throw new InvalidOperationException("Przedmiot jest wyprzedany.");

        if (player.Crystals < offerItem.Price)
            throw new InvalidOperationException("Brak kryształów.");

        var item = await _context.Items.FirstAsync(i => i.Code == itemCode);

        player.Crystals -= offerItem.Price;
        offerItem.Quantity--;

        await _inventoryService.AddItemToInventory(player.Id, item.Id);

        if (itemCode == "flashlight_basic")
            player.AddFlag("flashlight_bought");

        await _context.SaveChangesAsync();

        return player.Crystals;
    }

    private TradeOffer GetOffer(string tradeId)
    {
        return tradeId switch
        {
            "settlement_trader" => TradeOffersData.SettlementTrader,
            _ => throw new InvalidOperationException($"Unknown trade id: {tradeId}")
        };
    }
}
