using Application.Common;
using Application.Common.Interfaces;
using Application.Features.Auth.DTOs;
using Application.Interfaces;

namespace Application.Features.Auth.Commands;

public record SyncUserCommand(Guid SubUid, string UserName) : ICommand<Result<SyncUserDto>>;

public class SyncUser(IUserRepository userRepository) : ICommandHandler<SyncUserCommand, Result<SyncUserDto>>
{
    public async Task<Result<SyncUserDto>> Handle(SyncUserCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var isNewUser = await userRepository.SyncUserFromIdPAsync(
                command.SubUid, 
                command.UserName, 
                cancellationToken
            );
            
            return Result<SyncUserDto>.Success(isNewUser);
        }
        catch
        {
            return Result<SyncUserDto>.Failure("An error occurred while retrieving user");
        }
    }
}