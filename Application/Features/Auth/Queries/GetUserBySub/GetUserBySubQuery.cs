using Application.Common.Interfaces;
using Application.Common.Interfaces.Handlers;
using Application.Common.Interfaces.Handlers.Messaging;
using Application.Features.Auth.DTOs;
using Domain.Common;

namespace Application.Features.Auth.Queries.GetUserBySub;

public record GetUserByIdPSubQuery(Guid Sub) : IQuery<Result<AuthenticatedUserDto>>;

public class GetUserBySubQueryHandler(IUserQueries userQueries) : IQueryHandler<GetUserByIdPSubQuery, Result<AuthenticatedUserDto>>
{
    public async Task<Result<AuthenticatedUserDto>> Handle(GetUserByIdPSubQuery query, CancellationToken cancellationToken)
    {
        var sub = query.Sub;

        if (sub == Guid.Empty)
            return Result<AuthenticatedUserDto>.Failure("Can't search for user without a valid uid");
        
        var user = await userQueries.GetUserBySubAsync(sub, cancellationToken);
        
        if(user == null)
            return Result<AuthenticatedUserDto>.Failure("User not found");

        var userResponse = new AuthenticatedUserDto(user.Id, user.SubUid, user.UserName, user.CreatedAt);
        return Result<AuthenticatedUserDto>.Success(userResponse);
    }
}