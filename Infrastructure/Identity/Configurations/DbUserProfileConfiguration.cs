using Infrastructure.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Identity.Configurations;

public class DbUserProfileConfiguration : IEntityTypeConfiguration<DbUserProfile>
{
    public void Configure(EntityTypeBuilder<DbUserProfile> builder)
    {
        builder.HasOne(p => p.User)
            .WithOne(u => u.Profile)
            .HasForeignKey<DbUserProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Stories)
            .WithOne(s => s.UserProfile)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}