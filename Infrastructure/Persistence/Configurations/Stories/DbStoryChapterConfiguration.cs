using Domain.StoryManagement.Entities;
using Domain.StoryManagement.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Stories;

public class StoryChapterConfiguration : IEntityTypeConfiguration<StoryChapter>
{
    public void Configure(EntityTypeBuilder<StoryChapter> builder)
    {
        // Table mapping
        builder.ToTable("chapters", "story");
        
        // Primary key
        builder.HasKey(c => c.Id);
        
        // Map ChapterId value object
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .HasConversion(
                id => id.Value,
                value => ChapterId.From(value)
            )
            .HasDefaultValueSql("gen_random_uuid()");
        
        // Map StoryId value object (foreign key)
        builder.Property(c => c.StoryId)
            .HasColumnName("story_id")
            .HasConversion(
                id => id.Value,
                value => StoryId.From(value)
            )
            .IsRequired();
        
        // Map properties
        builder.Property(c => c.ChapterNumber)
            .HasColumnName("chapter_number")
            .IsRequired();
        
        builder.Property(c => c.Title)
            .HasColumnName("title")
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(c => c.Body)
            .HasColumnName("body")
            .HasColumnType("text")
            .IsRequired();
        
        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("NOW()")
            .IsRequired();
        
        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("NOW()")
            .IsRequired();
        
        // Shadow properties for soft delete (if you want to add them)
        builder.Property<bool>("IsPrivate")
            .HasColumnName("is_private")
            .HasDefaultValue(false)
            .IsRequired();
        
        builder.Property<bool>("IsDeleted")
            .HasColumnName("is_deleted")
            .HasDefaultValue(false)
            .IsRequired();
        
        builder.Property<DateTime?>("DeletedAt")
            .HasColumnName("deleted_at");
        
        // Query filter for soft delete
        builder.HasQueryFilter(c => EF.Property<bool>(c, "IsDeleted") == false);
        
        // Indexes (matching your DB)
        builder.HasIndex(c => c.StoryId)
            .HasDatabaseName("idx_story_chapter_story_id");
        
        // Unique index on story_id + chapter_number (for non-deleted chapters)
        builder.HasIndex(c => new { c.StoryId, c.ChapterNumber })
            .HasDatabaseName("idx_story_chapter_unique")
            .IsUnique()
            .HasFilter("is_deleted = FALSE");
        
        // Ordering index
        builder.HasIndex(c => new { c.StoryId, c.ChapterNumber })
            .HasDatabaseName("idx_story_chapters_ordering")
            .HasFilter("is_deleted = FALSE");
    }
}



