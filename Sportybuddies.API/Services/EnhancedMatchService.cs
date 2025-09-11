using Sportybuddies.API.Common.Patterns;
using Sportybuddies.API.Services.Domain;

namespace Sportybuddies.API.Services;

public interface IEnhancedMatchService
{
    Task<Result<MatchDto>> GetRandomMatchAsync(Guid profileId);
    Task<Result> ProcessMatchesForProfileAsync(Guid profileId);
}

public class EnhancedMatchService(
    IMatchFilteringDomainService matchFilteringDomainService,
    IProfilesRepository profilesRepository,
    IMatchesRepository matchesRepository,
    ApplicationDbContext dbContext) : IEnhancedMatchService
{
    public async Task<Result<MatchDto>> GetRandomMatchAsync(Guid profileId)
    {
        return await matchFilteringDomainService.GetRandomValidMatchAsync(profileId);
    }

    public async Task<Result> ProcessMatchesForProfileAsync(Guid profileId)
    {
        try
        {
            // Add new matches
            await FindMatchesToAddAsync(profileId);
            
            // Remove invalid matches
            await FindMatchesToRemoveAsync(profileId);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error processing matches: {ex.Message}");
        }
    }

    private async Task FindMatchesToAddAsync(Guid profileId)
    {
        var profile = await profilesRepository.GetProfileByIdWithSportsAsync(profileId);
        if (profile == null)
            return;
            
        if (profile.Sports.Count == 0)
            return;

        var potentialMatches = await profilesRepository.GetPotentialMatchesAsync(profileId, profile.Sports.Select(s => s.Id));

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

    private async Task FindMatchesToRemoveAsync(Guid profileId)
    {
        await matchesRepository.RemoveInvalidMatchesForProfileAsync(profileId);
        await dbContext.SaveChangesAsync();
    }
}