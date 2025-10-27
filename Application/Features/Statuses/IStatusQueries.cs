using Application.Features.Statuses.DTOs;

namespace Application.Features.Statuses;

public interface IStatusQueries
{
    Task<IReadOnlyList<StatusDto>> GetAllAsync(CancellationToken cancellationToken);
}