
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Game;

public class InventoryItem
{
    public int Id { get; set; }

    public int PlayerId { get; set; }
    public Player Player { get; set; } = null!;

    public int ItemId { get; set; }

    public Item Item { get; set; } = null!;

    public bool IsEquipped { get; set; } = false;

    public int Quantity { get; set; } = 1;
}
