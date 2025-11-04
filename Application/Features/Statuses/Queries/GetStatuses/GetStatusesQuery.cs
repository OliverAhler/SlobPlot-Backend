using Application.Common.Interfaces;
using Application.Common.Interfaces.Handlers;
using Application.Common.Interfaces.Handlers.Messaging;
using Application.Features.Statuses.DTOs;
using Domain.Common;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Features.Statuses.Queries.GetStatuses;

public record GetStatusesQuery() : IQuery<Result<IReadOnlyList<StatusDto>>>;

public class GetStatusesQueryHandler(IStatusQueries statusQueries, IMemoryCache cache) 
    : IQueryHandler<GetStatusesQuery, Result<IReadOnlyList<StatusDto>>>
{
    private const string CacheKey = "all_story_statuses";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);
    
    public async Task<Result<IReadOnlyList<StatusDto>>> Handle(GetStatusesQuery query, CancellationToken cancellationToken)
    {
        // Fix: IReadOnlyList instead of List
        if (cache.TryGetValue(CacheKey, out IReadOnlyList<StatusDto>? cachedStatuses) && cachedStatuses != null)
        {
            return Result<IReadOnlyList<StatusDto>>.Success(cachedStatuses);
        }
        
        var result = await statusQueries.GetAllAsync(cancellationToken);
        
        if (result.Count == 0)
            return Result<IReadOnlyList<StatusDto>>.Failure("No statuses found");
        
        cache.Set(CacheKey, result, CacheDuration);
        
        return Result<IReadOnlyList<StatusDto>>.Success(result);
    }
}