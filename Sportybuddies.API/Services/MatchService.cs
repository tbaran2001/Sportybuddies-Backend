namespace Sportybuddies.API.Services;

public class MatchService(
    IProfilesRepository profilesRepository,
    IMatchesRepository matchesRepository,
    ApplicationDbContext dbContext)
    : IMatchService
{
    public async Task FindMatchesToAddAsync(Guid profileId)
    {
        var profile = await profilesRepository.GetProfileByIdWithSportsAsync(profileId);
        if (profile == null)
            throw new Exception("Profile not found");
        if (profile.Sports.Count == 0)
            return;

        var potentialMatches =
            await profilesRepository.GetPotentialMatchesAsync(profileId, profile.Sports.Select(s => s.Id));

        var newMatches = new List<Match>();

        foreach (var matchedProfile in potentialMatches)
        {
            var (match1, match2) = Match.CreatePair(profileId, matchedProfile.Id, DateTime.UtcNow);
            newMatches.Add(match1);
            newMatches.Add(match2);
        }

        await matchesRepository.AddMatchesAsync(newMatches);
        await dbContext.SaveChangesAsync();
    }

    public async Task FindMatchesToRemoveAsync(Guid profileId)
    {
        await matchesRepository.RemoveInvalidMatchesForProfileAsync(profileId);
        await dbContext.SaveChangesAsync();
    }

    public async Task<MatchDto> GetRandomMatchAsync(Guid profileId)
    {
        var profile = await profilesRepository.GetProfileByIdAsync(profileId);
        if (profile == null)
            throw new ProfileNotFoundException(profileId);

        var preferences = profile.Preferences;
        if (preferences == null)
            throw new InvalidOperationException("Profile preferences are not defined.");

        var profileMatches = await matchesRepository.GetActiveProfileMatchesAsync(profileId);

        var profileMatchesList = profileMatches.ToList();
        if (profileMatchesList.Count == 0)
            return null;

        var validMatches = new List<MatchDto>();

        foreach (var match in profileMatchesList)
        {
            var matchedProfile = await profilesRepository.GetProfileByIdWithSportsAsync(match.MatchedProfileId);
            if (matchedProfile == null)
                continue;

            var age = CalculateAge(matchedProfile.DateOfBirth);
            if (age < preferences.MinAge || age > preferences.MaxAge)
                continue;

            if (preferences.PreferredGender != Gender.Unknown && matchedProfile.Gender != preferences.PreferredGender)
                continue;

            if (profile.Location == null || matchedProfile.Location == null)
                continue;

            var distance = profile.Location.CalculateDistance(matchedProfile.Location);
            if (distance > preferences.MaxDistance)
                continue;

            var matchDto = new MatchDto(
                Id: match.Id,
                OppositeMatchId: match.OppositeMatchId,
                ProfileId: profileId,
                MatchedProfile: matchedProfile.Adapt<ProfileDto>(),
                MatchDateTime: match.MatchDateTime,
                Swipe: match.Swipe,
                SwipeDateTime: match.SwipeDateTime,
                Distance: distance
            );

            validMatches.Add(matchDto);
        }

        if (!validMatches.Any())
            return null;

        var random = new Random();
        return validMatches[random.Next(validMatches.Count)];
    }

    private int CalculateAge(DateTimeOffset dateOfBirth)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - dateOfBirth.Year;
        if (DateOnly.FromDateTime(dateOfBirth.Date) > today.AddYears(-age))
            age--;
        return age;
    }
}