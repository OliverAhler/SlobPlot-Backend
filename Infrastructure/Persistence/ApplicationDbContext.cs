using Infrastructure.Persistence.Entities.Identity;
using Infrastructure.Persistence.Entities.Master;
using Infrastructure.Persistence.Entities.Story;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    //Master
    public DbSet<DbGenre> Genre { get; set; } = null!;
    public DbSet<DbStoryStatus> Status { get; set; } = null!;
    
    //Identity
    public DbSet<DbUser> Users { get; set; } = null!;
    public DbSet<DbUserProfile> UserProfiles { get; set; } = null!;
    
    //Story
    public DbSet<DbStory> Stories { get; set; } = null!;
    public DbSet<DbChapter> Chapters { get; set; } = null!;
    public DbSet<DbStoryGenre> StoryGenres { get; set; } = null!;
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}