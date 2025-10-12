using Application.Common;
using Application.Common.Interfaces;
using Application.Features.Auth.DTOs;
using Application.Interfaces;

namespace Application.Features.Auth.Commands;

public record SyncUserCommand(Guid SubUid, string UserName) : ICommand<Result<bool>>;

public class SyncUser(IUserRepository userRepository) : ICommandHandler<SyncUserCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(SyncUserCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var isNewUser = await userRepository.SyncUserFromIdPAsync(
                command.SubUid, 
                command.UserName, 
                cancellationToken
            );
            
            return Result<bool>.Success(isNewUser);
        }
        catch
        {
            return Result<bool>.Failure("An error occurred while retrieving user");
        }
    }
}