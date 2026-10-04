using Application.Common.Interfaces;
using Application.IRepositories;
using Domain.Common.Authorization;
using Domain.StoryManagement.ValueObjects;
using Vesia.Dispatch;
using Vesia.Result;

namespace Application.Features.Stories.Commands.AddChapter;

public record CreateChapterCommand(Guid StoryId,  string Title, string Body, bool IsPublic = true) : ICommand<Result<Guid>>;

public class AddChapterCommandHandler(IStoryRepository storyRepository, IAuthorizationService authorizationService, IUnitOfWork unitOfWork) : ICommandHandler<CreateChapterCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateChapterCommand command, CancellationToken cancellationToken)
    {
        var storyId = StoryId.From(command.StoryId);

        var storyAuthResult = await authorizationService.AuthorizeAndFetch(
            ct => storyRepository.GetStoryById(storyId, ct),
            story => story.AuthorId,
            AuthorizationPolicy.PublicOrOwner,
            cancellationToken
        );
        
        if (!storyAuthResult.IsSuccess)
            return Result<Guid>.Failure(storyAuthResult.Error ?? Error.Internal("Unknown Error"));

        var story = storyAuthResult.Value;
        var result = story.AddChapter(command.Title, command.Body, command.IsPublic);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result<Guid>.Success(result.Value.Id.Value);
    }
}
