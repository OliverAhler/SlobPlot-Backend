using Domain.StoryManagement.Aggregates;
using Domain.StoryManagement.Entities;
using Domain.StoryManagement.ValueObjects;
using Domain.UserManagement.ValueObjects;
using Infrastructure.Master.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Stories.Configurations;

public class StoryConfiguration : IEntityTypeConfiguration<Story>
{
    public void Configure(EntityTypeBuilder<Story> builder)
    {
        // Table mapping
        builder.ToTable("stories", "story");

        // Primary key - convert StoryId value object to Guid
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasColumnName("id")
            .HasConversion(
                id => id.Value,
                value => StoryId.From(value));

        // AuthorId - convert UserId value object to Guid, map to user_profile_id column
        builder.Property(s => s.AuthorId)
            .HasColumnName("user_profile_id")
            .IsRequired()
            .HasConversion(
                userId => userId.Value,
                value => UserId.From(value));

        // Properties
        builder.Property(s => s.Title)
            .HasColumnName("title")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.SubTitle)
            .HasColumnName("subtitle")
            .HasMaxLength(255);

        builder.Property(s => s.Summary)
            .HasColumnName("summary")
            .HasMaxLength(1500);

        builder.Property(s => s.IsPublic)
            .HasColumnName("is_public")
            .IsRequired();

        builder.Property(s => s.StoryStatusId)
            .HasColumnName("story_status_id")
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.Property<DateTime?>("DeletedAt")
            .HasColumnName("deleted_at");

        // Global query filter for soft delete
        builder.HasQueryFilter(s => EF.Property<DateTime?>(s, "DeletedAt") == null);

        // Chapters collection - owned collection with cascade delete
        builder.HasMany(s => s.Chapters)
            .WithOne()
            .HasForeignKey(c => c.StoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure access to private _chapters field
        builder.Metadata.FindNavigation(nameof(Story.Chapters))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        // Navigation property to UserProfile (Author)
        builder.HasOne(s => s.Author)
            .WithMany()
            .HasForeignKey(s => s.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(s => s.Author)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Navigation property for Genres (many-to-many through junction table)
        builder.HasMany(s => s.Genres)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "story_genres",
                j => j.HasOne<Genre>()
                    .WithMany()
                    .HasForeignKey("genre_id")
                    .OnDelete(DeleteBehavior.Restrict),
                j => j.HasOne<Story>()
                    .WithMany()
                    .HasForeignKey("story_id")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.ToTable("story_genres", "story");
                    j.HasKey("story_id", "genre_id");
                });

        builder.Navigation(s => s.Genres)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Relationship with Status (DbStatus)
        builder.HasOne<DbStatus>()
            .WithMany()
            .HasForeignKey(s => s.StoryStatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
