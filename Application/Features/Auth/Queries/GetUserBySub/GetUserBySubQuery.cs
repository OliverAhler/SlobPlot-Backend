using Application.Common.Interfaces;
using Application.Features.Auth.DTOs;
using Domain.Common;

namespace Application.Features.Auth.Queries.GetUserBySub;

public record GetUserByIdPSubQuery(Guid Sub) : IQuery<Result<UserDto>>;

public class GetUserBySubQueryHandler(IUserQueries userQueries) : IQueryHandler<GetUserByIdPSubQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetUserByIdPSubQuery query, CancellationToken cancellationToken)
    {
        var sub = query.Sub;

        if (sub == Guid.Empty)
            return Result<UserDto>.Failure("Can't search for user without a valid uid");
        
        var user = await userQueries.GetUserBySubAsync(sub, cancellationToken);
        
        if(user == null)
            return Result<UserDto>.Failure("User not found");

        var userResponse = new UserDto(user.Id, user.SubUid, user.UserName, user.CreatedAt);
        return Result<UserDto>.Success(userResponse);
    }
}