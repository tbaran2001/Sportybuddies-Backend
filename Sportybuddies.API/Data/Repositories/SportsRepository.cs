namespace Sportybuddies.API.Data.Repositories;

public class SportsRepository(ApplicationDbContext dbContext) : ISportsRepository
{
    public async Task AddSportAsync(Sport sport, CancellationToken cancellationToken = default)
    {
        await dbContext.Sports.AddAsync(sport, cancellationToken);
    }

    public async Task<IEnumerable<Sport>> GetAllSportsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Sports.ToListAsync(cancellationToken);
    }

    public async Task<Sport> GetSportByIdAsync(Guid sportId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Sports.FirstOrDefaultAsync(s => s.Id == sportId, cancellationToken);
    }

    public async Task<bool> SportNameExistsAsync(string sportName, CancellationToken cancellationToken = default)
    {
        return await dbContext.Sports.AnyAsync(s => s.Name == sportName, cancellationToken);
    }
}