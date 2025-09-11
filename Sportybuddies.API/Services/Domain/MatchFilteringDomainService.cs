using Sportybuddies.API.Common.Patterns;
using Sportybuddies.API.Modules.Matches.Dtos;
using Sportybuddies.API.Modules.Profiles.Models;

namespace Sportybuddies.API.Services.Domain;

public interface IMatchFilteringDomainService
{
    Task<Result<MatchDto>> GetRandomValidMatchAsync(Guid profileId);
}

public class MatchFilteringDomainService(
    IProfilesRepository profilesRepository,
    IMatchesRepository matchesRepository,
    IMatchFilterStrategy matchFilterStrategy) : IMatchFilteringDomainService
{
    public async Task<Result<MatchDto>> GetRandomValidMatchAsync(Guid profileId)
    {
        var profile = await profilesRepository.GetProfileByIdAsync(profileId);
        if (profile == null)
            return Result.Failure<MatchDto>("Profile not found");

        if (profile.Preferences == null)
            return Result.Failure<MatchDto>("Profile preferences are not defined");

        var profileMatches = await matchesRepository.GetActiveProfileMatchesAsync(profileId);
        var profileMatchesList = profileMatches.ToList();
        
        if (profileMatchesList.Count == 0)
            return Result.Failure<MatchDto>("No matches available");

        var validMatches = new List<MatchDto>();

        foreach (var match in profileMatchesList)
        {
            var matchedProfile = await profilesRepository.GetProfileByIdWithSportsAsync(match.MatchedProfileId);
            if (matchedProfile == null) continue;

            var distance = profile.Location?.CalculateDistance(matchedProfile.Location) ?? 0;
            
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

            if (matchFilterStrategy.IsMatch(profile, matchedProfile, matchDto))
            {
                validMatches.Add(matchDto);
            }
        }

        if (!validMatches.Any())
            return Result.Failure<MatchDto>("No valid matches found");

        var random = new Random();
        var selectedMatch = validMatches[random.Next(validMatches.Count)];
        
        return Result.Success(selectedMatch);
    }
}