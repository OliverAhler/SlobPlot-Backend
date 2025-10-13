using Application.Common.Interfaces;
using Domain.Aggregates.Users;
using Domain.Common;
using Domain.IRepositories;

namespace Application.Features.Auth.Commands.SyncUser;

public record SyncUserCommand(Guid SubUid, string UserName) : ICommand<Result<bool>>;

public class SyncUserCommandHandler(IUserRepository userRepository, IUserProfileRepository userProfileRepository) : ICommandHandler<SyncUserCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(SyncUserCommand command, CancellationToken cancellationToken)
    {
        try
        {
            // Check if user exists
            var existingUser = await userRepository.GetUserBySubAsync(command.SubUid, cancellationToken);
        
            //Check updates if user exists
            if (existingUser != null)
            {
                existingUser.UpdateUserName(command.UserName);
                
                if (existingUser.UserName != command.UserName)
                    await userRepository.UpdateAsync(existingUser, cancellationToken);
            
                return Result<bool>.Success(false); // Not a new user
            }
            
            // Create new user
            var userResult = User.Create(command.SubUid, command.UserName);
        
            if (!userResult.IsSuccess)
                return Result<bool>.Failure(userResult.Error);
        
            var newUser = userResult.Value;
            
            var profileResult = UserProfile.Create(newUser.Id, command.UserName);
        
            if (!profileResult.IsSuccess)
                return Result<bool>.Failure(profileResult.Error);
            
            await userRepository.AddAsync(newUser, cancellationToken);
            await userProfileRepository.AddAsync(profileResult.Value, cancellationToken);
        
            return Result<bool>.Success(true); // New user created
        }
        catch
        {
            return Result<bool>.Failure("An error occurred while retrieving user");
        }
    }
}