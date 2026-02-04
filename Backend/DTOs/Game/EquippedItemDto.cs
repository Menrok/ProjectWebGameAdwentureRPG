using Backend.Models.Game;

namespace Backend.DTOs.Game;

public class EquippedItemDto
{
    public int InventoryItemId { get; set; }
    public EquipmentSlot Slot { get; set; }
    public string Name { get; set; } = "";
    public string Icon { get; set; } = "";
}
