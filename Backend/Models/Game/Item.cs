using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Game;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public ItemType ItemType { get; set; }

    public EquipmentSlot? Slot { get; set; }

    public int AttackBonus { get; set; }
    public int DefenseBonus { get; set; }

    public int HealAmount { get; set; }
}
