using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Game;
public class Player
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public Auth.User User { get; set; } = null!;

    [Required]
    public string Name { get; set; } = null!;

    public int Level { get; set; } = 1;
    public int Experience { get; set; } = 0;

    public int Health { get; set; } = 100;
    public int MaxHealth { get; set; } = 100;
}
