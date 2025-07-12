namespace Sportybuddies.API.Modules.Profiles.Exceptions.Domain;

public class ProfileAlreadyHasSportException(Guid profileId, Guid sportId)
    : ConflictException($"Profile with ID '{profileId}' already has sport with ID '{sportId}'");