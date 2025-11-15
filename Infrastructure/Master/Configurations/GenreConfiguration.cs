using Domain.StoryManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Master.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        // Table mapping
        builder.ToTable("genres", "master");

        // Primary key
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id)
            .HasColumnName("id");

        // Properties
        builder.Property(g => g.DisplayName)
            .HasColumnName("display_name")
            .HasMaxLength(100)
            .IsRequired();
    }
}
