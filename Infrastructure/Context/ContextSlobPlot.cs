using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class ContextSlobPlot(DbContextOptions<ContextSlobPlot> options) : DbContext(options)
{
    public DbSet<DbUser> Users { get; set; } = null!;
    public DbSet<DbProfileIcon> ProfileIcons { get; set; } = null!;
    public DbSet<DbUserProfile> UserProfiles { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<DbUser>(entity =>
        {
            entity.HasOne(user => user.UserProfile)
                .WithOne(profile => profile.User)
                .HasForeignKey<DbUserProfile>(profile => profile.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DbUserProfile>(entity =>
        {
            entity.HasOne(userProfile => userProfile.ProfileIcon)
                .WithMany()  // No reverse navigation on ProfileIcon
                .HasForeignKey(userProfile => userProfile.ProfileIconId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}


