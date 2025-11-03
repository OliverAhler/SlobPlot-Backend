using Infrastructure.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Identity.Configurations;

public class DbUserConfiguration : IEntityTypeConfiguration<DbUser>
{

    public void Configure(EntityTypeBuilder<DbUser> builder)
    {
        builder.HasOne(u => u.Profile)
            .WithOne(p => p.User)
            .HasForeignKey<DbUserProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}