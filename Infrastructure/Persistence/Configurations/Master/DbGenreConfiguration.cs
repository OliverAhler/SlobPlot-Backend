using Infrastructure.Persistence.Entities.Master;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Master;

public class DbGenreConfiguration : IEntityTypeConfiguration<DbGenre>
{
    public void Configure(EntityTypeBuilder<DbGenre> builder)
    {
        builder.HasMany(genre => genre.StoryGenres)
            .WithOne(storyGenre => storyGenre.Genre)
            .HasForeignKey(storyGenre => storyGenre.GenreId) 
            .OnDelete(DeleteBehavior.Restrict);
    }
}
