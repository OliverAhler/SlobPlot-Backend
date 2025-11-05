using Infrastructure.Master.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Master.Configurations;

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
