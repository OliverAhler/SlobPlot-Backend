using Application.Common.Interfaces;
using Application.IRepositories;
using Domain.Aggregates.Users;
using Domain.Common;

namespace Application.Features.Auth.Commands.SyncUser;

public record SyncUserCommand(Guid SubUid, string UserName) : ICommand<Result<bool>>;

public class SyncUserCommandHandler(IUserRepository userRepository, IUserProfileRepository userProfileRepository, IUnitOfWork context) : ICommandHandler<SyncUserCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(SyncUserCommand command, CancellationToken cancellationToken)
    {
        // Check if user exists
        var existingUser = await userRepository.GetUserBySubAsync(command.SubUid, cancellationToken);
    
        //Check updates if user exists
        if (existingUser != null)
        {
            existingUser.UpdateUserName(command.UserName);
            userRepository.UpdateUser(existingUser);
        
            await context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(false); // Not a new user
        }
        
        // Create new user
        var userResult = User.Create(command.SubUid, command.UserName);
    
        if (!userResult.IsSuccess)
            return Result<bool>.Failure(userResult.Error);
    
        var newUser = userResult.Value;
        
        var profileResult = UserProfile.Create(newUser.Id!, command.UserName);
    
        if (!profileResult.IsSuccess)
            return Result<bool>.Failure(profileResult.Error);
        
        userRepository.AddUser(newUser);
        userProfileRepository.AddUserProfile(profileResult.Value);
        await context.SaveChangesAsync(cancellationToken);
    
        return Result<bool>.Success(true); // New user created
    }
}