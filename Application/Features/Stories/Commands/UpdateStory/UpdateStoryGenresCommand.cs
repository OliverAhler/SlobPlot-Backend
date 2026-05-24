using Application.Common.Interfaces;
using Application.Features.Genres;
using Application.IRepositories;
using Domain.Common;
using Domain.Common.Authorization;
using Domain.StoryManagement.ValueObjects;
using Venly.Dispatch.Interfaces;
using Venly.Dispatch.Interfaces.Messaging;

namespace Application.Features.Stories.Commands.UpdateStory;

public record UpdateStoryGenresCommand(Guid  StoryId, int[] GenreIds) : ICommand<Result>;

public class UpdateStoryGenresCommandHandler(
    IStoryRepository storyRepository,
    IGenreRepository genreRepository,
    IUnitOfWork unitOfWork,
    IAuthorizationService authorizationService) : ICommandHandler<UpdateStoryGenresCommand, Result>
{
    public async Task<Result> Handle(UpdateStoryGenresCommand command, CancellationToken cancellationToken)
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
        
        var genres = await genreRepository.GetByIdsAsync(command.GenreIds, cancellationToken);
        
        var story = storyAuthResult.Value;
        story.UpdateGenres(genres.ToArray());

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}