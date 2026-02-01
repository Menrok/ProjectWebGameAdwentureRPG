using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Game;

public class PlayerFlag
{
    [Key]
    public int Id { get; set; }

    public int PlayerId { get; set; }
    public Player Player { get; set; } = null!;

    [Required]
    public string Flag { get; set; } = "";
}
