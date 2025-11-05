using Application.Common.Interfaces.Handlers;
using Application.Common.Interfaces.Handlers.Messaging;
using Application.IRepositories;
using Domain.Common;
using Domain.StoryManagement.ValueObjects;

namespace Application.Features.Stories.Commands.UpdateStory;

public record UpdateStoryPrivacyCommand(Guid StoryId, bool IsPrivate) : ICommand<Result>;

public class UpdateStoryPrivacyHandler(IStoryRepository storyRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateStoryPrivacyCommand, Result>
{
    public async Task<Result> Handle(UpdateStoryPrivacyCommand command, CancellationToken cancellationToken)
    {
        var storyId = StoryId.From(command.StoryId);
        var story = await storyRepository.GetStoryById(storyId, cancellationToken);
        
        if (story is null)
            return Result.Failure("Story not found");
        
        story.UpdatePrivacy(command.IsPrivate);
        storyRepository.UpdateStory(story);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}