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

    public async Task<IEnumerable<Profile>> GetPotentialMatchesAsync(Guid profileId, IEnumerable<Guid> profileSports)
    {
        return await dbContext.Profiles
            .Where(p => p.Id != profileId)
            .Where(p => p.Sports.Any(s => profileSports.Contains(s.Id)))
            .Where(p => !dbContext.Matches.Any(m =>
                (m.ProfileId == profileId && m.MatchedProfileId == p.Id) ||
                (m.ProfileId == p.Id && m.MatchedProfileId == profileId)))
            .ToListAsync();
    }
}