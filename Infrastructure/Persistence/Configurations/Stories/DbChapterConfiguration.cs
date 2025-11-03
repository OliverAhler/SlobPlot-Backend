using Infrastructure.Persistence.Entities.Stories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Stories;

public class DbChapterConfiguration : IEntityTypeConfiguration<DbChapter>
{
    public void Configure(EntityTypeBuilder<DbChapter> builder)
    {
        builder.HasOne(c => c.Story)
            .WithMany(s => s.Chapters)
            .HasForeignKey(c => c.StoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}