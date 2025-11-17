using Application.Common.Interfaces;
using Application.Common.Interfaces.Handlers;
using Application.Common.Interfaces.Handlers.Messaging;
using Application.IRepositories;
using Domain.Common;
using Domain.Common.Authorization;
using Domain.StoryManagement.ValueObjects;

namespace Application.Features.Stories.Commands.UpdateStory;

public record UpdateStoryGenresCommand(Guid  StoryId, int[] GenreIds) : ICommand<Result>;

public class UpdateStoryGenresCommandHandler(IStoryRepository storyRepository, IUnitOfWork unitOfWork, IAuthorizationService authorizationService) : ICommandHandler<UpdateStoryGenresCommand, Result>
{
    public async Task<Result> Handle(UpdateStoryGenresCommand command, CancellationToken cancellationToken)
    {
        var storyId = StoryId.From(command.StoryId);

        var storyAuthResult = await authorizationService.AuthorizeAndFetch(
            ct => storyRepository.GetStoryById(storyId, ct),
            story => story.AuthorId,
            AuthorizationPolicy.PublicOrOwner,
            cancellationToken
        );

        if (!storyAuthResult.IsSuccess)
            return storyAuthResult;
        
        var story = storyAuthResult.Value;
        story.UpdateGenres(command.GenreIds);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}