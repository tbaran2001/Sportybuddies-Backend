namespace Sportybuddies.API.Modules.Matches.Features.EventHandlers;

public class UpdateMatchesWhenProfileSportAddedHandler(IMatchService matchService)
    : INotificationHandler<ProfileSportAddedDomainEvent>
{
    public async Task Handle(ProfileSportAddedDomainEvent notification, CancellationToken cancellationToken)
    {
        await matchService.FindMatchesToAddAsync(notification.ProfileId);
    }
}