using Application.Features.Genres.DTOs;
using Domain.Common;
using Microsoft.Extensions.Caching.Memory;
using Venly.Dispatch.Interfaces;
using Venly.Dispatch.Interfaces.Messaging;

namespace Application.Features.Genres.Queries.GetGenres;
public record GetGenresQuery() : IQuery<Result<IReadOnlyList<GenreDto>>>;

public class GetGenresQueryHandler(IGenreQueries genreQueries, IMemoryCache cache) 
    : IQueryHandler<GetGenresQuery, Result<IReadOnlyList<GenreDto>>>
{
    private const string CacheKey = "all_genres";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24); 
    
    public async Task<Result<IReadOnlyList<GenreDto>>> Handle(GetGenresQuery query, CancellationToken cancellationToken)
    {
        // Try to get from cache (cast to IReadOnlyList)
        if (cache.TryGetValue(CacheKey, out IReadOnlyList<GenreDto>? cachedGenres) && cachedGenres != null)
        {
            return Result<IReadOnlyList<GenreDto>>.Success(cachedGenres);
        }
        
        // Cache miss - fetch from database
        var genres = await genreQueries.GetAllAsync(cancellationToken);
        
        if (genres.Count == 0)
            return Result<IReadOnlyList<GenreDto>>.Failure("No genres found");
        
        cache.Set(CacheKey, genres, CacheDuration);
        
        return Result<IReadOnlyList<GenreDto>>.Success(genres);
    }
}