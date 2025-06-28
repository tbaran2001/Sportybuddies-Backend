namespace Sportybuddies.API.Data.Repositories;

public interface ISportsRepository
{
    Task AddSportAsync(Sport sport);
    Task<IEnumerable<Sport>> GetAllSportsAsync();
    Task<bool> SportNameExistsAsync(string sportName);
}