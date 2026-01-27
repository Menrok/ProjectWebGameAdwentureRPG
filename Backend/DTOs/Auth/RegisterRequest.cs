using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Auth;

public class RegisterRequest
{
    [Required]
    [MinLength(3)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Hasła nie są takie same")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
