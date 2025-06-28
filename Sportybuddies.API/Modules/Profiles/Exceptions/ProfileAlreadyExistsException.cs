namespace Sportybuddies.API.Modules.Profiles.Exceptions;

public class ProfileAlreadyExistsException(Guid userId)
    : ConflictException($"Profile for user with ID '{userId}' already exists.");