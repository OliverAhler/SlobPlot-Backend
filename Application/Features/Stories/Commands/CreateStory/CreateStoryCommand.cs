using Application.Features.Genres;
using Application.IRepositories;
using Domain.StoryManagement.Aggregates;
using Domain.UserManagement.ValueObjects;
using Vesia.Dispatch;
using Vesia.Result;

namespace Application.Features.Stories.Commands.CreateStory;

public record CreateStoryCommand(Guid UserId, string Title, string SubTitle, string Summary, bool IsPrivate, int[] GenreIds) : ICommand<Result<Guid>>;

public class CreateStoryCommandHandler(IStoryRepository storyRepository, IGenreRepository genreRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateStoryCommand, Result<Guid>> {
    
    public async Task<Result<Guid>> Handle(CreateStoryCommand command, CancellationToken cancellationToken)
    {
        var userId = UserId.From(command.UserId);
        
        var genres = await genreRepository.GetByIdsAsync(command.GenreIds, cancellationToken);
        
        var storyResult = Story.Create(userId, command.Title, command.SubTitle, command.Summary, command.IsPrivate, genres.ToArray());
        
        if(!storyResult.IsSuccess)
            return Result<Guid>.Failure(storyResult.Error ?? Error.Internal("Unknown Error"));

        var story = storyResult.Value;

        storyRepository.AddStory(story);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result<Guid>.Success(story.Id.Value);
    }
}
