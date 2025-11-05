using Application.IRepositories;

namespace Infrastructure.Persistence;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);
}