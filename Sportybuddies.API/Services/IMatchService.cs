namespace Sportybuddies.API.Services;

public interface IMatchService
{
    Task FindMatchesToAddAsync(Guid profileId);
    Task FindMatchesToRemoveAsync(Guid profileId);
    Task<MatchDto> GetRandomMatchAsync(Guid profileId);
}