namespace Sportybuddies.API.Data.Repositories;

public class SportsRepository(ApplicationDbContext dbContext) : ISportsRepository
{
    public async Task<IEnumerable<Sport>> GetAllSportsAsync()
    {
        return await dbContext.Sports.ToListAsync();
    }

    public async Task<Sport> GetSportByIdAsync(Guid sportId)
    {
        return await dbContext.Sports.FindAsync(sportId);
    }

    public async Task AddSportAsync(Sport sport)
    {
        await dbContext.Sports.AddAsync(sport);
    }

    public void RemoveSport(Sport sport)
    {
        dbContext.Sports.Remove(sport);
    }

    public async Task<bool> SportNameExistsAsync(string sportName)
    {
        return await dbContext.Sports.AnyAsync(s => s.Name == sportName);
    }
}