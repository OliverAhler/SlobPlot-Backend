using Application.Features.Genres;
using Domain.StoryManagement.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Master.Repositories;

public class GenreRepository(ApplicationDbContext context) : IGenreRepository
{
    public async Task<IReadOnlyList<Genre>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Genres
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Genre>> GetByIdsAsync(IEnumerable<int> genreIds, CancellationToken cancellationToken)
    {
        return await context.Genres
            .Where(g => genreIds.Contains(g.Id))
            .ToListAsync(cancellationToken);
    }
}