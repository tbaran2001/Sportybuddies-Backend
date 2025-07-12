namespace Sportybuddies.API.Modules.Profiles.Exceptions.Domain;

public class ProfileDoesNotHaveSportException(Guid profileId, Guid sportId)
    : NotFoundException($"Profile with ID '{profileId}' does not have sport with ID '{sportId}'");