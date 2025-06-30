namespace Sportybuddies.API.Data.Repositories;

public class ProfilesRepository(ApplicationDbContext dbContext) : IProfilesRepository
{
    public async Task<Profile> GetProfileByIdAsync(Guid profileId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);
    }

    public async Task<Profile> GetProfileByIdWithSportsAsync(Guid profileId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Profiles
            .Include(p => p.Sports)
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);
    }

    public async Task<IEnumerable<Profile>> GetAllProfilesWthSportsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Profiles
            .Include(p => p.Sports)
            .ToListAsync(cancellationToken);
    }

    public async Task AddProfileAsync(Profile profile, CancellationToken cancellationToken = default)
    {
        await dbContext.Profiles.AddAsync(profile, cancellationToken);
    }
}