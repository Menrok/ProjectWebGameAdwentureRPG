using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Game;

public class Item
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = "";
    public string Icon { get; set; } = "";

    public ItemType ItemType { get; set; }

    public EquipmentSlot? Slot { get; set; }

    public int? MinDamage { get; set; }
    public int? MaxDamage { get; set; }
    
    public int DefenseBonus { get; set; }

    public int HealAmount { get; set; }

}
