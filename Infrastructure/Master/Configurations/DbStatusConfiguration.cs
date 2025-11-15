using Infrastructure.Master.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Master.Configurations;

public class DbStatusConfiguration : IEntityTypeConfiguration<DbStatus>
{
    public void Configure(EntityTypeBuilder<DbStatus> builder)
    {
        // No additional configuration needed - using data annotations
    }
}