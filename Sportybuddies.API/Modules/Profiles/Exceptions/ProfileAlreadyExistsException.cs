namespace Sportybuddies.API.Modules.Profiles.Exceptions;

public class ProfileAlreadyExistsException(string userId)
    : ConflictException($"Profile for user with ID '{userId}' already exists.");