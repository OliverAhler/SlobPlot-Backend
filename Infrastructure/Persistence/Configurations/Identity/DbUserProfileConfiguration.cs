using Domain.UserManagement.Entities;
using Domain.UserManagement.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Identity;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        // Table mapping
        builder.ToTable("user_profiles", "auth");
        
        // Primary key (shared with User - maps to user_id column)
        builder.HasKey(p => p.Id);
        
        // Map UserId value object to user_id column
        builder.Property(p => p.Id)
            .HasColumnName("user_id")
            .HasConversion(
                id => id.Value,                    // UserId → Guid
                value => UserId.From(value)        // Guid → UserId
            );
        
        builder.Property(p => p.DisplayName)
            .HasColumnName("display_name")
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(p => p.Bio)
            .HasColumnName("bio")
            .HasMaxLength(5000);
        
        builder.Property(p => p.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();
    }
}