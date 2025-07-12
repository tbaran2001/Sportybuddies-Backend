namespace Sportybuddies.API.Modules.Matches.Exceptions;

public class MatchesNotFoundException(Guid profileId)
    : NotFoundException($"Matches not found for profile with ID: {profileId}");