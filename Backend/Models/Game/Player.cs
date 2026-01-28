using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    public int BaseAttack { get; set; }
    public int BaseDefense { get; set; }

    public int? EquippedWeaponId { get; set; }
    public Item? EquippedWeapon { get; set; }

    public int? EquippedClothingId { get; set; }
    public Item? EquippedClothing { get; set; }

    public ICollection<InventoryItem> Inventory { get; set; } = new List<InventoryItem>();
    
    public StoryChapter CurrentChapter { get; set; } = StoryChapter.Prologue;
    public string? CurrentStoryNode { get; set; }
    public string CurrentLocationId { get; set; } = null!;
    public List<string> Flags { get; set; } = new();
}
