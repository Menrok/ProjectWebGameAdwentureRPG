using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Backend.Models.Game;

public class Player
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public Auth.User User { get; set; } = null!;

    [Required]
    public string Name { get; set; } = null!;

    public int Health { get; set; }
    public int MaxHealth { get; set; }

    public int BaseMinAttack { get; set; }
    public int BaseMaxAttack { get; set; }
    public int BaseDefense { get; set; }

    public int Crystals { get; set; }

    public ICollection<InventoryItem> Inventory { get; set; } = new List<InventoryItem>();
    
    public string? CurrentStoryNodeId { get; set; }
    public string CurrentLocationId { get; set; } = null!;

    public ICollection<PlayerFlag> Flags { get; set; } = new List<PlayerFlag>();
    public bool HasFlag(string flag) => Flags.Any(f => f.Flag == flag);
    public void AddFlag(string flag)
    {
        if (!HasFlag(flag))
            Flags.Add(new PlayerFlag { Flag = flag });
    }
    public void RemoveFlagsByPrefix(string prefix)
    {
        var toRemove = Flags.Where(f => f.Flag.StartsWith(prefix)).ToList();

        foreach (var flag in toRemove)
        {
            Flags.Remove(flag);
        }
    }

    public List<PlayerQuest> Quests { get; set; } = new();
}
