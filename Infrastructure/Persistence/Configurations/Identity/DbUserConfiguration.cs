using Domain.UserManagement.Aggregates;
using Domain.UserManagement.Entities;
using Domain.UserManagement.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Identity;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Table mapping
        builder.ToTable("users", "auth");
        
        // Primary key
        builder.HasKey(u => u.Id);
        
        // Map UserId value object to Guid column
        builder.Property(u => u.Id)
            .HasColumnName("id")
            .HasConversion(
                id => id.Value,                    // UserId → Guid
                value => UserId.From(value)        // Guid → UserId
            );
        
        builder.Property(u => u.SubUid)
            .HasColumnName("sub_uid")
            .IsRequired();
        
        builder.Property(u => u.UserName)
            .HasColumnName("user_name")
            .HasMaxLength(30)
            .IsRequired();
        
        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
        
        builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();
        
        // Shadow properties for soft delete
        builder.Property<bool>("IsDeleted")
            .HasColumnName("is_deleted")
            .HasDefaultValue(false);
        
        builder.Property<DateTime?>("DeletedAt")
            .HasColumnName("deleted_at");
        
        // One-to-One with UserProfile (shared PK)
        builder.HasOne<UserProfile>("_profile")
            .WithOne()
            .HasForeignKey<UserProfile>(p => p.Id)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();  // Profile must always exist
        
        // Query filter for soft delete
        builder.HasQueryFilter(u => EF.Property<bool>(u, "IsDeleted") == false);
        
        builder.HasIndex(u => u.SubUid)
            .HasDatabaseName("idx_users_sub_uid");
        
        builder.HasIndex(u => u.UserName)
            .HasDatabaseName("idx_users_user_name");
    }
}