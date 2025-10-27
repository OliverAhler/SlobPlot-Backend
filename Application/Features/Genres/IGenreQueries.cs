using Application.Features.Genres.DTOs;

namespace Application.Features.Genres;

public interface IGenreQueries
{
    Task<IReadOnlyList<GenreDto>> GetAllAsync(CancellationToken cancellationToken);
}