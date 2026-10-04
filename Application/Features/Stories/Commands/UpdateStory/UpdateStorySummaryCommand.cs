using Application.Common.Interfaces;
using Application.IRepositories;
using Domain.Common.Authorization;
using Domain.StoryManagement.ValueObjects;
using Vesia.Dispatch;
using Vesia.Result;

namespace Application.Features.Stories.Commands.UpdateStory;

public record UpdateStorySummaryCommand(Guid  StoryId, string Summary) : ICommand<Result>;

public class UpdateStorySummaryCommandHandler(IStoryRepository storyRepository, IUnitOfWork unitOfWork, IAuthorizationService authorizationService) : ICommandHandler<UpdateStorySummaryCommand, Result>
{
    public async Task<Result> Handle(UpdateStorySummaryCommand command, CancellationToken cancellationToken)
    {
        var storyId = StoryId.From(command.StoryId);

        var storyAuthResult = await authorizationService.AuthorizeAndFetch(
            ct => storyRepository.GetStoryById(storyId, ct),
            story => story.AuthorId,
            AuthorizationPolicy.MustBeOwner,
            cancellationToken
        );

        if (!storyAuthResult.IsSuccess)
            return Result.Failure(storyAuthResult.Error);
        
        var story = storyAuthResult.Value;
        story.UpdateSummary(command.Summary);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}