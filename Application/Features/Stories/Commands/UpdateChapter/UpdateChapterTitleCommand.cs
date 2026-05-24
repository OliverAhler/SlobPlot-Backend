using Application.Common.Interfaces;
using Application.IRepositories;
using Domain.Common;
using Domain.Common.Authorization;
using Domain.StoryManagement.ValueObjects;
using Venly.Dispatch.Interfaces;
using Venly.Dispatch.Interfaces.Messaging;

namespace Application.Features.Stories.Commands.UpdateChapter;

public record UpdateChapterTitleCommand(Guid  StoryId, Guid ChapterId, string Title) : ICommand<Result>;

public class UpdateChapterTitleCommandHandler(IStoryRepository storyRepository, IUnitOfWork unitOfWork, IAuthorizationService authorizationService) : ICommandHandler<UpdateChapterTitleCommand, Result>
{
    public async Task<Result> Handle(UpdateChapterTitleCommand command, CancellationToken cancellationToken)
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
        
        var chapterId = ChapterId.From(command.ChapterId);
        
        var story = storyAuthResult.Value;
        story.UpdateChapterTitle(chapterId, command.Title);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}