using Sportybuddies.API.Common.Interfaces;
using Sportybuddies.API.Common.Patterns;

namespace Sportybuddies.API.Data.Repositories;

public interface IProfilesRepository : IAsyncRepository<Profile>
{
    // Keep existing methods for backward compatibility
    Task<Profile> GetProfileByIdAsync(Guid profileId, CancellationToken cancellationToken = default);
    Task<Profile> GetProfileByIdWithSportsAsync(Guid profileId, CancellationToken cancellationToken = default);
    Task<Profile> GetProfileByUserIdWithSportsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Profile>> GetAllProfilesWthSportsAsync(CancellationToken cancellationToken = default);
    Task AddProfileAsync(Profile profile, CancellationToken cancellationToken = default);
    Task<IEnumerable<Profile>> GetPotentialMatchesAsync(Guid profileId, IEnumerable<Guid> profileSports);
    
    // New specification-based methods
    Task<Profile> GetProfileBySpecificationAsync(ISpecification<Profile> specification, CancellationToken cancellationToken = default);
    Task<IEnumerable<Profile>> GetProfilesBySpecificationAsync(ISpecification<Profile> specification, CancellationToken cancellationToken = default);
}