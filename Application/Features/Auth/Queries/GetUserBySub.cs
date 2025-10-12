using Application.Common.Interfaces;
using Application.Features.Auth.DTOs;
using Domain.Common;
using Domain.IRepositories;

namespace Application.Features.Auth.Queries;

public record GetUserByIdPSubQuery(Guid Sub) : IQuery<Result<UserDto>>;

public class GetUserBySubQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUserByIdPSubQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetUserByIdPSubQuery query, CancellationToken cancellationToken)
    {
        var sub = query.Sub;

        if (sub == Guid.Empty)
            return Result<UserDto>.Failure("Can't search for user without a valid uid");

        try
        {
            var user = await userRepository.GetUserBySubAsync(sub, cancellationToken);
            
            if(user == null)
                return Result<UserDto>.Failure("User not found");

            var dto = new UserDto(user.Id.Value, user.SubUid, user.UserName, user.CreatedAt);
            
            return Result<UserDto>.Success(dto);
        }
        catch
        {
            return Result<UserDto>.Failure("An error occurred while retrieving user");
        }
    }
}