using Application.Common.Interfaces;
using Domain.Common;
using Domain.IRepositories;

namespace Application.Features.Auth.Queries.GetUserBySub;

public record GetUserByIdPSubQuery(Guid Sub) : IQuery<Result<GetUserBySubResponse>>;

public class GetUserBySubQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUserByIdPSubQuery, Result<GetUserBySubResponse>>
{
    public async Task<Result<GetUserBySubResponse>> Handle(GetUserByIdPSubQuery query, CancellationToken cancellationToken)
    {
        var sub = query.Sub;

        if (sub == Guid.Empty)
            return Result<GetUserBySubResponse>.Failure("Can't search for user without a valid uid");

        try
        {
            var user = await userRepository.GetUserBySubAsync(sub, cancellationToken);
            
            if(user == null)
                return Result<GetUserBySubResponse>.Failure("User not found");

            var dto = new GetUserBySubResponse(user.Id.Value, user.SubUid, user.UserName, user.CreatedAt);
            
            return Result<GetUserBySubResponse>.Success(dto);
        }
        catch
        {
            return Result<GetUserBySubResponse>.Failure("An error occurred while retrieving user");
        }
    }
}