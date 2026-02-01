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
    public DbSet<Quest> Quests => Set<Quest>();
    public DbSet<PlayerQuest> PlayerQuests => Set<PlayerQuest>();

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

        modelBuilder.Entity<InventoryItem>()
            .HasIndex(i => new { i.PlayerId, i.SlotIndex })
            .IsUnique();

        modelBuilder.Entity<PlayerQuest>()
            .HasKey(pq => new { pq.PlayerId, pq.QuestId });

        modelBuilder.Entity<PlayerQuest>()
            .HasOne(pq => pq.Player)
            .WithMany(p => p.Quests)
            .HasForeignKey(pq => pq.PlayerId);

        modelBuilder.Entity<PlayerQuest>()
            .HasOne(pq => pq.Quest)
            .WithMany()
            .HasForeignKey(pq => pq.QuestId);

        modelBuilder.Entity<PlayerFlag>()
            .HasOne(pf => pf.Player)
            .WithMany(p => p.Flags)
            .HasForeignKey(pf => pf.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
