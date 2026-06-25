using Microsoft.EntityFrameworkCore;

namespace API.Data;


public class VideoProjectorDbContext : DbContext
{
    public VideoProjectorDbContext(DbContextOptions<VideoProjectorDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}