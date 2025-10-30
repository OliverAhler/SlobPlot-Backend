using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Stories;

public class DbStoryGenreConfiguration : IEntityTypeConfiguration<DbStoryGenre>
{
    public void Configure(EntityTypeBuilder<DbStoryGenre> builder)
    {
        builder.HasKey(sg => new { sg.StoryId, sg.GenreId });
        
        builder.HasOne(sg => sg.Genre)
            .WithMany()
            .HasForeignKey(sg => sg.GenreId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}