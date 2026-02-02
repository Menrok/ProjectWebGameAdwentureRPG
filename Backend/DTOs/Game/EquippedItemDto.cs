using Backend.Models.Game;

namespace Backend.DTOs.Game;

public class EquippedItemDto
{
    public EquipmentSlot Slot { get; set; }
    public string Name { get; set; } = "";
    public string Icon { get; set; } = "";
}
