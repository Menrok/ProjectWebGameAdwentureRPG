namespace Backend.DTOs.Game;

public class InventoryItemDto
{
    public int Id { get; set; }
    public int? SlotIndex { get; set; }
    public ItemDto Item { get; set; } = null!;
    public bool IsEquipped { get; set; }
}