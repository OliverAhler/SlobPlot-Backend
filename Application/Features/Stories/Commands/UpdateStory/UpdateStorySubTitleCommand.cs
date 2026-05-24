using Application.Common.Interfaces;
using Application.IRepositories;
using Domain.Common;
using Domain.Common.Authorization;
using Domain.StoryManagement.ValueObjects;
using Venly.Dispatch.Interfaces;
using Venly.Dispatch.Interfaces.Messaging;

namespace Application.Features.Stories.Commands.UpdateStory;

public record UpdateStorySubTitleCommand(Guid  StoryId, string SubTitle) : ICommand<Result>;

public class UpdateStorySubTitleCommandHandler(IStoryRepository storyRepository, IUnitOfWork unitOfWork, IAuthorizationService authorizationService) : ICommandHandler<UpdateStorySubTitleCommand, Result>
{
    public async Task<Result> Handle(UpdateStorySubTitleCommand command, CancellationToken cancellationToken)
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
        story.UpdateSubTitle(command.SubTitle);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}