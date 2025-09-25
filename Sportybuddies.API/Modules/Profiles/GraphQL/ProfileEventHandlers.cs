//#nullable enable
//using HotChocolate.Subscriptions;

//namespace Sportybuddies.API.Modules.Profiles.GraphQL;

/// <summary>
/// Event handler to publish GraphQL subscription events for profile sport operations
/// </summary>
/// Note: Temporarily commented out to get basic GraphQL working
/*
internal class ProfileSportDomainEventHandler(ITopicEventSender eventSender)
    : INotificationHandler<ProfileSportAddedDomainEvent>, INotificationHandler<ProfileSportRemovedDomainEvent>
{
    public async Task Handle(ProfileSportAddedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Create the event payload - we'll need to get sport name from repository
        var profileSportEvent = new ProfileSportEvent(
            notification.ProfileId, 
            notification.SportId, 
            "Sport"); // For now, we'll use a placeholder - could enhance to fetch sport name
            
        await eventSender.SendAsync("profile_sport_added", profileSportEvent, cancellationToken);
    }

    public async Task Handle(ProfileSportRemovedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Create the event payload
        var profileSportEvent = new ProfileSportEvent(
            notification.ProfileId, 
            notification.SportId, 
            "Sport"); // For now, we'll use a placeholder - could enhance to fetch sport name
            
        await eventSender.SendAsync("profile_sport_removed", profileSportEvent, cancellationToken);
    }
}

/// <summary>
/// Enhanced event handler that publishes profile updates for subscriptions
/// </summary>
internal class ProfileUpdatedEventHandler(
    ITopicEventSender eventSender, 
    IProfilesRepository profilesRepository)
    : INotificationHandler<ProfileSportAddedDomainEvent>, INotificationHandler<ProfileSportRemovedDomainEvent>
{
    public async Task Handle(ProfileSportAddedDomainEvent notification, CancellationToken cancellationToken)
    {
        await PublishProfileUpdate(notification.ProfileId, cancellationToken);
    }

    public async Task Handle(ProfileSportRemovedDomainEvent notification, CancellationToken cancellationToken)
    {
        await PublishProfileUpdate(notification.ProfileId, cancellationToken);
    }

    private async Task PublishProfileUpdate(Guid profileId, CancellationToken cancellationToken)
    {
        var profile = await profilesRepository.GetProfileByIdWithSportsAsync(profileId, cancellationToken);
        if (profile != null)
        {
            var profileDto = profile.Adapt<ProfileDto>();
            await eventSender.SendAsync("profile_updated", profileDto, cancellationToken);
        }
    }
}
*/