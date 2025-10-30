using Domain.StoryManagement.Aggregates;
using Domain.StoryManagement.Entities;
using Domain.UserManagement.Aggregates;
using Domain.UserManagement.Entities;
using Infrastructure.Persistence.Configurations.Identity;
using Infrastructure.Persistence.Configurations.Stories;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    //Master
    public DbSet<DbGenre> Genre { get; set; } = null!;
    public DbSet<DbStatus> Status { get; set; } = null!;
    
    //Identity
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserProfile> UserProfiles { get; set; } = null!;
    
    //Story
    public DbSet<Story> Stories { get; set; } = null!;
    public DbSet<StoryChapter> Chapters { get; set; } = null!;
    public DbSet<DbStoryGenre> StoryGenres { get; set; } = null!;
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserProfileConfiguration());
        
        modelBuilder.ApplyConfiguration(new StoryConfiguration());
        modelBuilder.ApplyConfiguration(new StoryChapterConfiguration());
        modelBuilder.ApplyConfiguration(new DbStoryGenreConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}