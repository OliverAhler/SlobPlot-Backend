using Infrastructure.Persistence.Entities.Story;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Story;

public class DbStoryGenreConfiguration : IEntityTypeConfiguration<DbStoryGenre>
{
    public void Configure(EntityTypeBuilder<DbStoryGenre> builder)
    {
        builder.HasKey(sg => new { sg.StoryId, sg.GenreId });
        
        builder.HasOne(sg => sg.Story)
            .WithMany(s => s.StoryGenres)
            .HasForeignKey(sg => sg.StoryId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(sg => sg.Genre)
            .WithMany(g => g.StoryGenres)
            .HasForeignKey(sg => sg.GenreId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}