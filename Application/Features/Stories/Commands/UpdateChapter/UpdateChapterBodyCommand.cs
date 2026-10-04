using Application.Common.Interfaces;
using Application.IRepositories;
using Domain.Common.Authorization;
using Domain.StoryManagement.ValueObjects;
using Vesia.Dispatch;
using Vesia.Result;

namespace Application.Features.Stories.Commands.UpdateChapter;

public record UpdateChapterBodyCommand(Guid  StoryId, Guid ChapterId, string Title) : ICommand<Result>;

public class UpdateChapterBodyCommandHandler(IStoryRepository storyRepository, IUnitOfWork unitOfWork, IAuthorizationService authorizationService) : ICommandHandler<UpdateChapterBodyCommand, Result>
{
    public async Task<Result> Handle(UpdateChapterBodyCommand command, CancellationToken cancellationToken)
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
        
        var chapterId = ChapterId.From(command.ChapterId);
        
        var story = storyAuthResult.Value;
        story.UpdateChapterBody(chapterId, command.Title);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}