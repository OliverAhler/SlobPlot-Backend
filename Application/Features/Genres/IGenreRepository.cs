using Domain.StoryManagement.Entities;

namespace Application.Features.Genres;

public interface IGenreRepository
{
    Task<IReadOnlyList<Genre>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<Genre>> GetByIdsAsync(IEnumerable<int> genreIds, CancellationToken cancellationToken);
}