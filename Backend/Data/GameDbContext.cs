using Microsoft.EntityFrameworkCore;
using Backend.Models.Auth;
using Backend.Models.Game;

namespace Backend.Data;

public class GameDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Player> Players => Set<Player>();


    public GameDbContext(DbContextOptions<GameDbContext> options)
        : base(options)
    {
    }
}
