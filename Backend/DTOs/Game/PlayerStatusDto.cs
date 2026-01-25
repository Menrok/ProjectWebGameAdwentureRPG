namespace Backend.DTOs.Game;

public class PlayerStatusDto
{
    public int PlayerId { get; set; }
    public string Name { get; set; } = "";

    public int Health { get; set; }
    public int MaxHealth { get; set; }

    public int Attack { get; set; }
    public int Defense { get; set; }

    public string? Weapon { get; set; }
    public string? Clothing { get; set; }

    public int InventoryCount { get; set; }
}
