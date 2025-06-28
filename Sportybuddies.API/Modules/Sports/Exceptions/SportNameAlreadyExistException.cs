namespace Sportybuddies.API.Modules.Sports.Exceptions;

public class SportNameAlreadyExistException(string name) : ConflictException($"Sport with name \"{name}\" already exist!");