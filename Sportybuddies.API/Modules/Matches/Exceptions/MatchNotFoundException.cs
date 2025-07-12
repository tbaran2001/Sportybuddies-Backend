namespace Sportybuddies.API.Modules.Matches.Exceptions;

public class MatchNotFoundException(Guid matchId) : NotFoundException("Match", matchId);