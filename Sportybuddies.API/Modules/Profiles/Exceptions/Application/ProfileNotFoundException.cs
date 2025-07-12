namespace Sportybuddies.API.Modules.Profiles.Exceptions.Application;

public class ProfileNotFoundException(Guid id) : NotFoundException("Profile", id);