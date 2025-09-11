using Sportybuddies.API.Common.Extensions;
using Sportybuddies.API.Common.Patterns;
using Sportybuddies.API.Modules.Profiles.Specifications;

namespace Sportybuddies.API.Data.Repositories;

public class ProfilesRepository(ApplicationDbContext dbContext) : IProfilesRepository
{
    // IAsyncRepository implementation
    public async Task<Profile> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Profiles.FindAsync(id);
    }

    public async Task<IReadOnlyList<Profile>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Profiles.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Profile>> ListAsync(ISpecification<Profile> spec, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(spec).ToListAsync(cancellationToken);
    }

    public async Task<Profile> FirstOrDefaultAsync(ISpecification<Profile> spec, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Profile> AddAsync(Profile entity, CancellationToken cancellationToken = default)
    {
        await dbContext.Profiles.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(Profile entity, CancellationToken cancellationToken = default)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Profile entity, CancellationToken cancellationToken = default)
    {
        dbContext.Profiles.Remove(entity);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(ISpecification<Profile> spec, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(spec).CountAsync(cancellationToken);
    }

    // Legacy methods for backward compatibility - now using specifications internally
    public async Task<Profile> GetProfileByIdAsync(Guid profileId, CancellationToken cancellationToken = default)
    {
        var spec = new ProfileByIdSpecification(profileId);
        return await FirstOrDefaultAsync(spec, cancellationToken);
    }

    public async Task<Profile> GetProfileByIdWithSportsAsync(Guid profileId,
        CancellationToken cancellationToken = default)
    {
        var spec = new ProfileByIdWithSportsSpecification(profileId);
        return await FirstOrDefaultAsync(spec, cancellationToken);
    }

    public async Task<Profile> GetProfileByUserIdWithSportsAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var spec = new ProfileByUserIdWithSportsSpecification(userId);
        return await FirstOrDefaultAsync(spec, cancellationToken);
    }

    public async Task<IEnumerable<Profile>> GetAllProfilesWthSportsAsync(CancellationToken cancellationToken = default)
    {
        var spec = new AllProfilesWithSportsSpecification();
        return await ListAsync(spec, cancellationToken);
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

    // New specification-based methods
    public async Task<Profile> GetProfileBySpecificationAsync(ISpecification<Profile> specification, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(specification, cancellationToken);
    }

    public async Task<IEnumerable<Profile>> GetProfilesBySpecificationAsync(ISpecification<Profile> specification, CancellationToken cancellationToken = default)
    {
        return await ListAsync(specification, cancellationToken);
    }

    // Helper method to apply specifications
    private IQueryable<Profile> ApplySpecification(ISpecification<Profile> spec)
    {
        return SpecificationEvaluator.GetQuery(dbContext.Profiles.AsQueryable(), spec);
    }
}