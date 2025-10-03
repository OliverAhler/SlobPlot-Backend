using Application.Common;
using Application.Common.Interfaces;
using Application.Features.Auth.DTOs;
using Application.Interfaces;

namespace Application.Features.Auth.Queries;

public record GetUserByUidQuery(Guid Uid) : IQuery<Result<UserProfileDto>>;

public class GetUserByUid(IUserRepository userRepository) : IQueryHandler<GetUserByUidQuery, Result<UserProfileDto>>
{
    public async Task<Result<UserProfileDto>> Handle(GetUserByUidQuery query, CancellationToken cancellationToken)
    {
        var uid = query.Uid;

        if (uid == Guid.Empty)
            return Result<UserProfileDto>.Failure("Can't search for user without a valid uid");

        try
        {
            var user = await userRepository.GetUserByUidAsync(uid, cancellationToken);
            
            if(user == null)
                return Result<UserProfileDto>.Failure("User not found");
            
            return Result<UserProfileDto>.Success(user);
        }
        catch
        {
            return Result<UserProfileDto>.Failure("An error occurred while retrieving user");
        }
    }
}