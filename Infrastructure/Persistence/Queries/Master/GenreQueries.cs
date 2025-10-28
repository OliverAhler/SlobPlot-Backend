using Application.Features.Genres;
using Application.Features.Genres.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries.Master;

public class GenreQueries(ApplicationDbContext context) : IGenreQueries
{
    public async Task<IReadOnlyList<GenreDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Genre
            .AsNoTracking()
            .Select(g => new GenreDto(g.Id, g.DisplayName))
            .ToListAsync(cancellationToken);
    }
}
