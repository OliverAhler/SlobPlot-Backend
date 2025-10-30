using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.StoryManagement.Aggregates;
using Domain.StoryManagement.Entities;
using Domain.StoryManagement.ValueObjects;
using Domain.UserManagement.Entities;
using Domain.UserManagement.ValueObjects;
using Infrastructure.Persistence.Entities;

namespace Infrastructure.Persistence.Configurations.Stories;

public class StoryConfiguration : IEntityTypeConfiguration<Story>
{
    public void Configure(EntityTypeBuilder<Story> builder)
    {
        // Table mapping
        builder.ToTable("stories", "story");
        
        // Primary key
        builder.HasKey(s => s.Id);
        
        // Map StoryId value object
        builder.Property(s => s.Id)
            .HasColumnName("id")
            .HasConversion(
                id => id.Value,
                value => StoryId.From(value)
            )
            .HasDefaultValueSql("gen_random_uuid()");
        
        // Map AuthorId (UserId value object)
        builder.Property(s => s.AuthorId)
            .HasColumnName("user_profile_id")
            .HasConversion(
                id => id.Value,
                value => UserId.From(value)
            )
            .IsRequired();
        
        // Map properties
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
        
        builder.Property(s => s.IsPrivate)
            .HasColumnName("is_private")
            .HasDefaultValue(false)
            .IsRequired();
        
        builder.Property(s => s.StoryStatusId)
            .HasColumnName("story_status_id")
            .IsRequired();
        
        builder.Property(s => s.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("NOW()")
            .IsRequired();
        
        builder.Property(s => s.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("NOW()")
            .IsRequired();
        
        // Shadow properties for soft delete
        builder.Property<bool>("IsDeleted")
            .HasColumnName("is_deleted")
            .HasDefaultValue(false)
            .IsRequired();
        
        builder.Property<DateTime?>("DeletedAt")
            .HasColumnName("deleted_at");
        
        // Chapters relationship
        builder.HasMany<StoryChapter>("_chapters")
            .WithOne()
            .HasForeignKey(c => c.StoryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Ignore(s => s.Chapters);
        builder.Ignore(s => s._genreIds);
        builder.Ignore(s => s.GenreIds);
        
        // Foreign key to UserProfile
        builder.HasOne<UserProfile>()
            .WithMany()
            .HasForeignKey(s => s.AuthorId)
            .HasPrincipalKey(up => up.Id)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Foreign key to Status (lookup table)
        builder.HasOne<DbStatus>()
            .WithMany()
            .HasForeignKey(s => s.StoryStatusId)
            .HasPrincipalKey(st => st.Id)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Query filter for soft delete
        builder.HasQueryFilter(s => EF.Property<bool>(s, "IsDeleted") == false);
        
        // Indexes (matching your DB)
        builder.HasIndex(s => s.AuthorId)
            .HasDatabaseName("idx_story_stories_user_id");
        
        builder.HasIndex(s => s.StoryStatusId)
            .HasDatabaseName("idx_story_stories_status");
        
        // Partial index for public stories (EF Core 8+ supports this)
        builder.HasIndex("IsPrivate", "IsDeleted")
            .HasDatabaseName("idx_story_stories_public")
            .HasFilter("is_private = FALSE AND is_deleted = FALSE");
        
        builder.HasIndex(s => s.AuthorId)
            .HasDatabaseName("idx_stories_active")
            .HasFilter("is_deleted = FALSE");
    }
}