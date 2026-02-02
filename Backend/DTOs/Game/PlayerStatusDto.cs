namespace Backend.DTOs.Game;

public class PlayerStatusDto
{
    public int PlayerId { get; set; }
    public string Name { get; set; } = "";

    public int Health { get; set; }
    public int MaxHealth { get; set; }

    public int MinAttack { get; set; }
    public int MaxAttack { get; set; }
    public int Defense { get; set; }

    public int Crystals { get; set; }

    public List<EquippedItemDto> EquippedItems { get; set; } = new();

    public int InventoryCount { get; set; }
}
