using Backend.Models.Game;

namespace Backend.DTOs.Game;

public class ItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Icon { get; set; } = "";

    public ItemType ItemType { get; set; }
    public EquipmentSlot? Slot { get; set; }

    public int? MinDamage { get; set; }
    public int? MaxDamage { get; set; }
    public int DefenseBonus { get; set; }
    public int? HealAmount { get; set; }
}
