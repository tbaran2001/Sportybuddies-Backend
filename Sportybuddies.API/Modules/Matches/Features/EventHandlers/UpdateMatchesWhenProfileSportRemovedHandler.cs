namespace Sportybuddies.API.Modules.Matches.Features.EventHandlers;

public class UpdateMatchesWhenProfileSportRemovedHandler(IMatchService matchService)
    : INotificationHandler<ProfileSportRemovedDomainEvent>
{
    public async Task Handle(ProfileSportRemovedDomainEvent notification, CancellationToken cancellationToken)
    {
        await matchService.FindMatchesToRemoveAsync(notification.ProfileId);
    }
}