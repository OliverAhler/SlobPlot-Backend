using Application.Common;
using Application.Common.Interfaces;
using Application.Features.Auth.DTOs;
using Application.Interfaces;

namespace Application.Features.Auth.Queries;

public record GetUserByIdPSubQuery(Guid Sub) : IQuery<Result<UserDto>>;

public class GetUserByIdPSub(IUserRepository userRepository) : IQueryHandler<GetUserByIdPSubQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetUserByIdPSubQuery query, CancellationToken cancellationToken)
    {
        var sub = query.Sub;

        if (sub == Guid.Empty)
            return Result<UserDto>.Failure("Can't search for user without a valid uid");

        try
        {
            var user = await userRepository.GetUserByIdPUidAsync(sub, cancellationToken);
            
            if(user == null)
                return Result<UserDto>.Failure("User not found");
            
            return Result<UserDto>.Success(user);
        }
        catch
        {
            return Result<UserDto>.Failure("An error occurred while retrieving user");
        }
    }
}