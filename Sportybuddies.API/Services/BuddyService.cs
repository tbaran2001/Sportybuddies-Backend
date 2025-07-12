namespace Sportybuddies.API.Services;

public class BuddyService(IBuddiesRepository buddiesRepository) : IBuddyService
{
    public async Task AddBuddyAsync(Match match, Match oppositeMatch)
    {
        if (match.Swipe == Swipe.Left || oppositeMatch.Swipe == Swipe.Left)
            return;

        if (match == oppositeMatch)
            throw new Exception("Match and opposite match cannot be the same");

        if (await buddiesRepository.AreProfilesAlreadyBuddiesAsync(match.ProfileId, oppositeMatch.ProfileId))
            throw new Exception("Profiles are already buddies");

        if (match.Swipe != Swipe.Right || oppositeMatch.Swipe != Swipe.Right)
            return;

        var now = DateTimeOffset.UtcNow;
        var (buddy, oppositeBuddy) = Buddy.CreatePair(match.ProfileId, oppositeMatch.ProfileId, now);

        await buddiesRepository.AddBuddyAsync(buddy);
        await buddiesRepository.AddBuddyAsync(oppositeBuddy);
    }
}