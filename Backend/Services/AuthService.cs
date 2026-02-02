using Backend.Data;
using Backend.Models.Auth;
using Backend.Models.Game;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Backend.Services;

public class AuthService
{
    private readonly GameDbContext _db;
    private readonly IConfiguration _config;
    private readonly PasswordHasher<User> _hasher = new();

    public AuthService(GameDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<bool> Register(string username, string password)
    {
        if (await _db.Users.AnyAsync(u => u.Username == username))
            return false;

        var user = new User
        {
        Username = username
        };

        user.PasswordHash = _hasher.HashPassword(user, password);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var player = new Player
        {
            UserId = user.Id,
            Name = "Lia",

            Health = 30,
            MaxHealth = 100,

            Crystals = 0,
            
            BaseMinAttack = 2,
            BaseMaxAttack = 4,
            BaseDefense = 0,

            CurrentStoryNodeId = "prologue_intro",
            CurrentLocationId = "beach"
        };

        _db.Players.Add(player);
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<User?> Login(string username, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
            return null;

        var result = _hasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            password
        );

        return result == PasswordVerificationResult.Success ? user : null;
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}