using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.Reflection;

namespace API.Data;


public class VideoProjectorDbContext : DbContext
{
    public VideoProjectorDbContext(DbContextOptions<VideoProjectorDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}