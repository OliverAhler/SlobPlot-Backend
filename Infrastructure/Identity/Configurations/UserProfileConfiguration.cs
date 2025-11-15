using Domain.UserManagement.Entities;
using Domain.UserManagement.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Identity.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        // Table mapping
        builder.ToTable("user_profiles", "auth");

        // Primary key - convert UserId value object to Guid
        // UserProfile uses UserId as its primary key (same as User.Id)
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasColumnName("user_id")
            .HasConversion(
                id => id.Value,
                value => UserId.From(value));

        // Properties
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
