namespace Sportybuddies.API.Modules.Matches.Dtos;

public record MatchDto(
    Guid Id,
    Guid OppositeMatchId,
    Guid ProfileId,
    ProfileDto MatchedProfile,
    DateTimeOffset MatchDateTime,
    Swipe? Swipe,
    DateTimeOffset? SwipeDateTime,
    double Distance);