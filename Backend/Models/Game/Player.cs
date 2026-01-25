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

    public int Health { get; set; } = 100;
    public int MaxHealth { get; set; } = 100;

    public int BaseAttack { get; set; } = 5;
    public int BaseDefense { get; set; } = 0;

    public int? EquippedWeaponId { get; set; }
    public Item? EquippedWeapon { get; set; }

    public int? EquippedClothingId { get; set; }
    public Item? EquippedClothing { get; set; }

    public ICollection<InventoryItem> Inventory { get; set; } = new List<InventoryItem>();
}
