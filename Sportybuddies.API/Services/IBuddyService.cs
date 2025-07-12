namespace Sportybuddies.API.Services;

public interface IBuddyService
{
    Task AddBuddyAsync(Match match, Match oppositeMatch);
}