using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entity;

namespace NotificationService.Repository.Data;

public class AppDbContext : DbContext
{
    private readonly string _connectionString;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}