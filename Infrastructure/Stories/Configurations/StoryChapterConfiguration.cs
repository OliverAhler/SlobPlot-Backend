using Domain.StoryManagement.Entities;
using Domain.StoryManagement.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Stories.Configurations;

public class StoryChapterConfiguration : IEntityTypeConfiguration<StoryChapter>
{
    public void Configure(EntityTypeBuilder<StoryChapter> builder)
    {
        // Table mapping
        builder.ToTable("chapters", "story");

        // Primary key - convert ChapterId value object to Guid
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .HasConversion(
                id => id.Value,
                value => ChapterId.From(value));

        // Foreign key - convert StoryId value object to Guid
        builder.Property(c => c.StoryId)
            .HasColumnName("story_id")
            .IsRequired()
            .HasConversion(
                storyId => storyId.Value,
                value => StoryId.From(value));

        // Properties
        builder.Property(c => c.ChapterNumber)
            .HasColumnName("chapter_number")
            .IsRequired();

        builder.Property(c => c.Title)
            .HasColumnName("title")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.Body)
            .HasColumnName("body")
            .IsRequired();

        builder.Property(c => c.IsPublic)
            .HasColumnName("is_public")
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        // Shadow properties for soft delete (not in domain model)
        builder.Property<bool>("IsDeleted")
            .HasColumnName("is_deleted")
            .HasDefaultValue(false);

        builder.Property<DateTime?>("DeletedAt")
            .HasColumnName("deleted_at");

        // Global query filter for soft delete
        builder.HasQueryFilter(c => !EF.Property<bool>(c, "IsDeleted"));
    }
}
