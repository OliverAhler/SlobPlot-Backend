using Application.Features.Statuses;
using Application.Features.Statuses.DTOs;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Master.Queries;

public class StatusQueries(ApplicationDbContext context)  : IStatusQueries
{
    public async Task<IReadOnlyList<StatusDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Status
            .AsNoTracking()
            .Select(s => new StatusDto(s.Id, s.DisplayName, s.Description))
            .ToListAsync(cancellationToken);
    }
}