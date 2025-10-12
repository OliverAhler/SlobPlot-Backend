using Infrastructure.Models.IDM;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class ContextSlobPlot(DbContextOptions<ContextSlobPlot> options) : DbContext(options)
{
    public DbSet<DbUser> Users { get; set; } = null!;
    public DbSet<DbUserProfile> UserProfiles { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<DbUser>(entity =>
        {
            entity.HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<DbUserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}


