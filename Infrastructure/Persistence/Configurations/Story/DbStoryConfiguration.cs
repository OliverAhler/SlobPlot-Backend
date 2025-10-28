using Infrastructure.Persistence.Entities.Stories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Story;

public class DbStoryConfiguration : IEntityTypeConfiguration<DbStory>
{
    public void Configure(EntityTypeBuilder<DbStory> builder)
    {
        builder.HasOne(s => s.UserProfile)
            .WithMany(u => u.Stories)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasOne(s => s.Status)
            .WithMany(ss => ss.Stories)
            .HasForeignKey(s => s.StoryStatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}