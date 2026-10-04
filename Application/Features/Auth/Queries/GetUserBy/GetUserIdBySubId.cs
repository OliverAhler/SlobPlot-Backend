using Vesia.Dispatch;
using Vesia.Result;

namespace Application.Features.Auth.Queries.GetUserIdBySubId;

public record GetUserIdBySubIdQuery(Guid Sub) : IQuery<Result<Guid>>;

public class GetUserIdBySubIdHandler(IUserQueries userQueries) : IQueryHandler<GetUserIdBySubIdQuery, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(GetUserIdBySubIdQuery query, CancellationToken cancellationToken)
    {
        if (query.Sub == Guid.Empty)
            return Result<Guid>.Failure("Invalid sub ID");
        
        var userId = await userQueries.GetUserIdBySubIdAsync(query.Sub, cancellationToken);
        
        if (userId == null)
            return Result<Guid>.Failure("User not found");
        
        return Result<Guid>.Success(userId.Value);
    }
}