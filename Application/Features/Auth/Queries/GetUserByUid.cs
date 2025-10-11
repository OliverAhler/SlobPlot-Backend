using Application.Common;
using Application.Common.Interfaces;
using Application.Features.Auth.DTOs;
using Application.Interfaces;

namespace Application.Features.Auth.Queries;

public record GetUserByUidQuery(Guid Uid) : IQuery<Result<UserDto>>;

public class GetUserByUid(IUserRepository userRepository) : IQueryHandler<GetUserByUidQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetUserByUidQuery query, CancellationToken cancellationToken)
    {
        var uid = query.Uid;

        if (uid == Guid.Empty)
            return Result<UserDto>.Failure("Can't search for user without a valid uid");

        try
        {
            var user = await userRepository.GetUserByUidAsync(uid, cancellationToken);
            
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