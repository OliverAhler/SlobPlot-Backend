using Application.Common.Interfaces;
using Application.Common.Interfaces.Handlers;
using Application.Common.Interfaces.Handlers.Messaging;
using Application.IRepositories;
using Domain.Common;
using Domain.UserManagement.Aggregates;

namespace Application.Features.Auth.Commands.SyncUser;

public record SyncUserCommand(Guid SubUid, string UserName) : ICommand<Result<bool>>;

public class SyncUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<SyncUserCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(SyncUserCommand command, CancellationToken cancellationToken)
    {
        // Check if user exists
        var existingUser = await userRepository.GetUserBySubAsync(command.SubUid, cancellationToken);
    
        //Check updates if user exists
        if (existingUser != null)
        {
            // Only update if changed
            if (existingUser.UserName == command.UserName) return Result<bool>.Success(false);
            
            existingUser.UpdateUserName(command.UserName);
            userRepository.UpdateUser(existingUser);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Result<bool>.Success(false);
        }
        
        // Create new user
        var userResult = User.Create(command.SubUid, command.UserName);
    
        if (!userResult.IsSuccess)
            return Result<bool>.Failure(userResult.Error);
        
        userRepository.AddUser(userResult.Value);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    
        return Result<bool>.Success(true); // New user created
    }
}