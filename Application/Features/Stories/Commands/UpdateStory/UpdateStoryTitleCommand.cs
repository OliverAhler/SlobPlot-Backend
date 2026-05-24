using Application.Common.Interfaces;
using Application.IRepositories;
using Domain.Common;
using Domain.Common.Authorization;
using Domain.StoryManagement.ValueObjects;
using Venly.Dispatch.Interfaces;
using Venly.Dispatch.Interfaces.Messaging;

namespace Application.Features.Stories.Commands.UpdateStory;

public record UpdateStoryTitleCommand(Guid  StoryId, string Title) : ICommand<Result>;

public class UpdateStoryTitleCommandHandler(IStoryRepository storyRepository, IUnitOfWork unitOfWork, IAuthorizationService authorizationService) : ICommandHandler<UpdateStoryTitleCommand, Result>
{
    public async Task<Result> Handle(UpdateStoryTitleCommand command, CancellationToken cancellationToken)
    {
        var storyId = StoryId.From(command.StoryId);

        var storyAuthResult = await authorizationService.AuthorizeAndFetch(
            ct => storyRepository.GetStoryById(storyId, ct),
            story => story.AuthorId,
            AuthorizationPolicy.MustBeOwner,
            cancellationToken
        );

        if (!storyAuthResult.IsSuccess)
            return storyAuthResult;
        
        var story = storyAuthResult.Value;
        story.UpdateTitle(command.Title);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}