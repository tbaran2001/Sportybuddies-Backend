namespace Sportybuddies.API.Data.Repositories;

public interface ISportsRepository
{
    Task AddSportAsync(Sport sport, CancellationToken cancellationToken = default);
    Task<IEnumerable<Sport>> GetAllSportsAsync(CancellationToken cancellationToken = default);
    Task<Sport> GetSportByIdAsync(Guid sportId, CancellationToken cancellationToken = default);
    Task<bool> SportNameExistsAsync(string sportName, CancellationToken cancellationToken = default);
}