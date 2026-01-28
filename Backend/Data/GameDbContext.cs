using Microsoft.EntityFrameworkCore;
using Backend.Models.Auth;
using Backend.Models.Game;

namespace Backend.Data;

public class GameDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Player> Players => Set<Player>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();

    public GameDbContext(DbContextOptions<GameDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<InventoryItem>()
            .HasOne(ii => ii.Player)
            .WithMany(p => p.Inventory)
            .HasForeignKey(ii => ii.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<InventoryItem>()
            .HasOne(ii => ii.Item)
            .WithMany()
            .HasForeignKey(ii => ii.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Player>()
            .HasOne(p => p.EquippedWeapon)
            .WithMany()
            .HasForeignKey(p => p.EquippedWeaponId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Player>()
            .HasOne(p => p.EquippedClothing)
            .WithMany()
            .HasForeignKey(p => p.EquippedClothingId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InventoryItem>()
            .HasIndex(i => new { i.PlayerId, i.SlotIndex })
            .IsUnique();
    }
}
