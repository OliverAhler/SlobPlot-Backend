using Infrastructure.Models.Story;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class ContextStories(DbContextOptions<ContextStories> options) : DbContext(options)
{
    public DbSet<DbStory> Stories { get; set; } = null!;
    public DbSet<DbChapter> Chapters { get; set; } = null!;
    public DbSet<DbStoryGenre> StoryGenres { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Story → User
        modelBuilder.Entity<DbStory>(entity =>
        {
            entity.HasOne(s => s.UserProfile)
                .WithMany(u => u.Stories)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(s => s.StoryStatus)
                .WithMany(ss => ss.Stories)
                .HasForeignKey(s => s.StoryStatusId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    
        // StoryGenre composite key + relationships
        modelBuilder.Entity<DbStoryGenre>(entity =>
        {
            entity.HasKey(sg => new { sg.StoryId, sg.GenreId });
        
            entity.HasOne(sg => sg.Story)
                .WithMany(s => s.StoryGenres)
                .HasForeignKey(sg => sg.StoryId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(sg => sg.Genre)
                .WithMany(g => g.StoryGenres)
                .HasForeignKey(sg => sg.GenreId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    
        // Chapter → Story
        modelBuilder.Entity<DbChapter>(entity =>
        {
            entity.HasOne(c => c.Story)
                .WithMany(s => s.Chapters)
                .HasForeignKey(c => c.StoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}