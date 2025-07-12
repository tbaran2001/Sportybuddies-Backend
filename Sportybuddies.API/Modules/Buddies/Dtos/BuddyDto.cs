namespace Sportybuddies.API.Modules.Buddies.Dtos;

public record BuddyDto(
    Guid Id,
    Guid OppositeBuddyId,
    Guid ProfileId,
    ProfileDto MatchedProfile,
    DateTimeOffset CreatedOn,
    Guid? ConversationId);