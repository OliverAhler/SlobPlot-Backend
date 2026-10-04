using Application.Common.Interfaces;
using Application.IRepositories;
using Domain.Common.Authorization;
using Domain.StoryManagement.ValueObjects;
using Vesia.Dispatch;
using Vesia.Result;

namespace Application.Features.Stories.Commands.UpdateStory;

public record UpdateStoryPrivacyCommand(Guid StoryId, bool IsPublic) : ICommand<Result>;

public class UpdateStoryPrivacyHandler(IStoryRepository storyRepository, IAuthorizationService authorizationService, IUnitOfWork unitOfWork) : ICommandHandler<UpdateStoryPrivacyCommand, Result>
{
    public async Task<Result> Handle(UpdateStoryPrivacyCommand command, CancellationToken cancellationToken)
    {
        var storyId = StoryId.From(command.StoryId);

        var storyAuthResult = await authorizationService.AuthorizeAndFetch(
            ct => storyRepository.GetStoryById(storyId, ct),
            story => story.AuthorId,
            AuthorizationPolicy.MustBeOwner,
            cancellationToken
        );

        if (!storyAuthResult.IsSuccess)
            return Result.Failure(storyAuthResult.Error ?? Error.Internal("Unknown Error"));

        var story = storyAuthResult.Value;
        
        story.UpdatePrivacy(command.IsPublic);
        storyRepository.UpdateStory(story);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}