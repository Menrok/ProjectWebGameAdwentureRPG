using Backend.Models.Game;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Auth;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public Player? Player { get; set; }

}