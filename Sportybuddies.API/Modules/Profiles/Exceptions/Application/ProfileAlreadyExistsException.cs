namespace Sportybuddies.API.Modules.Profiles.Exceptions.Application;

public class ProfileAlreadyExistsException(Guid userId)
    : ConflictException($"Profile for user with ID '{userId}' already exists.");