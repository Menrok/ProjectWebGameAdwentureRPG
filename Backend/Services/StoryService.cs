using Backend.Data;
using Backend.Models.Game;
using Backend.Story;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class StoryService
{
    private readonly GameDbContext _context;
    private readonly InventoryService _inventoryService;

    private const string BandageCode = "bandage_basic";

    public StoryService(
        GameDbContext context,
        InventoryService inventoryService)
    {
        _context = context;
        _inventoryService = inventoryService;
    }

    public async Task<StoryNode> GetCurrentNode(int playerId)
    {
        var player = await _context.Players
            .FirstOrDefaultAsync(p => p.Id == playerId);

        if (player == null)
            throw new Exception("Player not found");

        var node = PrologueStory.Nodes
            .FirstOrDefault(n => n.Id == player.CurrentStoryNode);

        if (node == null)
            throw new Exception("Story node not found");

        return node;
    }

    public async Task<StoryNode> Choose(int playerId, string nextNodeId)
    {
        var player = await _context.Players
            .FirstOrDefaultAsync(p => p.Id == playerId);

        if (player == null)
            throw new Exception("Player not found");

        var nextNode = PrologueStory.Nodes
            .FirstOrDefault(n => n.Id == nextNodeId);

        if (nextNode == null)
            throw new Exception("Story node not found");

        await HandleStoryEffects(player, nextNodeId);

        player.CurrentStoryNode = nextNodeId;
        await _context.SaveChangesAsync();

        return nextNode;
    }

    private async Task HandleStoryEffects(Player player, string nodeId)
    {
        if (nodeId == "prologue_give_bandage")
        {
            var bandage = await _context.Items
                .FirstOrDefaultAsync(i => i.Code == BandageCode);

            if (bandage == null)
                throw new Exception("Bandage item not found in database");

            await _inventoryService.AddItemToInventory(
                player.Id,
                bandage.Id
            );
        }

        if (nodeId == "prologue_free_mode")
        {
            player.CurrentChapter = StoryChapter.Chapter1;
            player.CurrentLocationId = "beach";
        }
    }
}
