using Domain.UserManagement.Aggregates;
using Domain.UserManagement.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Identity.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Table mapping
        builder.ToTable("users", "auth");

        // Primary key - convert UserId value object to Guid
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasColumnName("id")
            .HasConversion(
                id => id.Value,
                value => UserId.From(value));

        // Properties
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

        // Shadow properties for soft delete (not in domain model)
        builder.Property<bool>("IsDeleted")
            .HasColumnName("is_deleted")
            .HasDefaultValue(false);

        builder.Property<DateTime?>("DeletedAt")
            .HasColumnName("deleted_at");

        // Global query filter for soft delete
        builder.HasQueryFilter(u => !EF.Property<bool>(u, "IsDeleted"));

        // UserProfile - 1:1 relationship with cascade delete
        builder.HasOne(u => u.Profile)
            .WithOne()
            .HasForeignKey<Domain.UserManagement.Entities.UserProfile>(p => p.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure access to private _profile field
        builder.Navigation(u => u.Profile)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
