using Infrastructure.Models.Master;
using Infrastructure.Models.Story;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class ContextMaster(DbContextOptions<ContextMaster> options) : DbContext(options)
{
    public DbSet<DbGenre> Genre { get; set; } = null!;
    public DbSet<DbStoryStatus> Status { get; set; } = null!;
}