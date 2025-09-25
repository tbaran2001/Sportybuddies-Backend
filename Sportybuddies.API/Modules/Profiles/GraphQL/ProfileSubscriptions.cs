#nullable enable
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using HotChocolate.Subscriptions;

namespace Sportybuddies.API.Modules.Profiles.GraphQL;

/// <summary>
/// GraphQL subscriptions for the Profiles module
/// </summary>
[Authorize]
public class ProfileSubscriptions
{
    /// <summary>
    /// Subscribe to profile updates
    /// </summary>
    /// <param name="profileId">Optional profile ID to filter updates</param>
    /// <returns>Profile update events</returns>
    [Subscribe]
    [Topic("profile_updated")]
    public ProfileDto? ProfileUpdated(
        [EventMessage] ProfileDto profile,
        Guid? profileId = null)
    {
        // If profileId is specified, only return updates for that profile
        if (profileId.HasValue && profile.Id != profileId.Value)
            return null;
            
        return profile;
    }
    
    /// <summary>
    /// Subscribe to profile sport additions
    /// </summary>
    /// <param name="profileId">Optional profile ID to filter events</param>
    /// <returns>Profile sport added events</returns>
    [Subscribe]
    [Topic("profile_sport_added")]
    public ProfileSportEvent? ProfileSportAdded(
        [EventMessage] ProfileSportEvent profileSportEvent,
        Guid? profileId = null)
    {
        // If profileId is specified, only return events for that profile
        if (profileId.HasValue && profileSportEvent.ProfileId != profileId.Value)
            return null;
            
        return profileSportEvent;
    }
    
    /// <summary>
    /// Subscribe to profile sport removals
    /// </summary>
    /// <param name="profileId">Optional profile ID to filter events</param>
    /// <returns>Profile sport removed events</returns>
    [Subscribe]
    [Topic("profile_sport_removed")]
    public ProfileSportEvent? ProfileSportRemoved(
        [EventMessage] ProfileSportEvent profileSportEvent,
        Guid? profileId = null)
    {
        // If profileId is specified, only return events for that profile
        if (profileId.HasValue && profileSportEvent.ProfileId != profileId.Value)
            return null;
            
        return profileSportEvent;
    }
}

/// <summary>
/// Event payload for profile sport operations
/// </summary>
public record ProfileSportEvent(Guid ProfileId, Guid SportId, string SportName);

/// <summary>
/// GraphQL type for ProfileSportEvent
/// </summary>
public class ProfileSportEventType : ObjectType<ProfileSportEvent>
{
    protected override void Configure(IObjectTypeDescriptor<ProfileSportEvent> descriptor)
    {
        descriptor.Name("ProfileSportEvent");
        descriptor.Description("Event triggered when a sport is added or removed from a profile");
        
        descriptor.Field(e => e.ProfileId)
            .Type<NonNullType<UuidType>>()
            .Description("The ID of the profile");
            
        descriptor.Field(e => e.SportId)
            .Type<NonNullType<UuidType>>()
            .Description("The ID of the sport");
            
        descriptor.Field(e => e.SportName)
            .Type<NonNullType<StringType>>()
            .Description("The name of the sport");
    }
}