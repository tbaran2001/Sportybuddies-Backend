namespace Sportybuddies.API.Modules.Sports.Exceptions;

public class SportNotFoundException(Guid id) : NotFoundException("Sport", id);