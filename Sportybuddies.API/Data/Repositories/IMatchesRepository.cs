namespace Sportybuddies.API.Data.Repositories;

public interface IMatchesRepository
{
    Task<Match> GetMatchByIdAsync(Guid matchId);
    Task<IEnumerable<Match>> GetProfileMatchesAsync(Guid profileId);
    Task<IEnumerable<Match>> GetProfileExistingMatchesAsync(Guid profileId);
    Task AddMatchesAsync(IEnumerable<Match> matches);
    Task<Match> GetRandomMatchAsync(Guid profileId);
    Task RemoveRangeAsync(IEnumerable<Match> matches);
    Task RemoveInvalidMatchesForProfileAsync(Guid profileId);
    Task<bool> CheckIfMatchExistsAsync(Guid profileId, Guid matchedProfileId);
    Task<IEnumerable<Match>> GetActiveProfileMatchesAsync(Guid profileId);
}