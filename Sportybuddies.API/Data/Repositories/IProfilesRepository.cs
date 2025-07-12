namespace Sportybuddies.API.Data.Repositories;

public interface IProfilesRepository
{
    Task<Profile> GetProfileByIdAsync(Guid profileId, CancellationToken cancellationToken = default);
    Task<Profile> GetProfileByIdWithSportsAsync(Guid profileId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Profile>> GetAllProfilesWthSportsAsync(CancellationToken cancellationToken = default);
    Task AddProfileAsync(Profile profile, CancellationToken cancellationToken = default);
    Task<IEnumerable<Profile>> GetPotentialMatchesAsync(Guid profileId, IEnumerable<Guid> profileSports);
}