using Infrastructure.Master.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Master.Configurations;

public class DbStatusConfiguration : IEntityTypeConfiguration<DbStatus>
{
    public void Configure(EntityTypeBuilder<DbStatus> builder)
    {
        builder.HasMany(s => s.Stories)
            .WithOne(s => s.Status)
            .HasForeignKey(story => story.StoryStatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}