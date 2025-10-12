using Infrastructure.Models.IDM;
using Infrastructure.Models.Story;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class ContextUsers(DbContextOptions<ContextUsers> options) : DbContext(options)
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

        modelBuilder.Entity<DbUserProfile>(entity =>
        {
            entity.HasOne(p => p.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<DbUserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.Stories)
                .WithOne(s => s.UserProfile)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}


