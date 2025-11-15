using Application.Common.Interfaces;
using Application.Common.Interfaces.Handlers;
using Application.Common.Interfaces.Handlers.Messaging;
using Application.IRepositories;
using Domain.Common;
using Domain.Common.Authorization;
using Domain.StoryManagement.ValueObjects;

namespace Application.Features.Stories.Commands.UpdateStory;

public record UpdateStoryPrivacyCommand(Guid StoryId, bool IsPublic) : ICommand<Result>;

public class UpdateStoryPrivacyHandler(IStoryRepository storyRepository, IAuthorizationService authorizationService, IUnitOfWork unitOfWork) : ICommandHandler<UpdateStoryPrivacyCommand, Result>
{
    public async Task<Result> Handle(UpdateStoryPrivacyCommand command, CancellationToken cancellationToken)
    {
        var storyId = StoryId.From(command.StoryId);
        var story = await storyRepository.GetStoryById(storyId, cancellationToken);
        
        if (story is null)
            return Result.Failure("Story not found");
        
        var authResult = await authorizationService.Authorize(
            story,
            s => s.AuthorId,
            AuthorizationPolicy.MustBeOwner,
            cancellationToken
        );
        
        if (!authResult.IsSuccess)
            return authResult;
        
        story.UpdatePrivacy(command.IsPublic);
        storyRepository.UpdateStory(story);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}