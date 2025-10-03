using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class ContextSlobPlot(DbContextOptions<ContextSlobPlot> options) : DbContext(options)
{
    public DbSet<DbUser> Users { get; set; } = null!;
    public DbSet<DbIcon> Icons { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<DbUser>(entity =>
        {
            entity.HasOne(user => user.Icon)
                .WithMany()
                .HasForeignKey(profile => profile.IconId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}


