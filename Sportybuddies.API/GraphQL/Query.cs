using HotChocolate.Authorization;
using Sportybuddies.API.Modules.Profiles.Features.Queries;

namespace Sportybuddies.API.GraphQL;

public class Query
{
    /// <summary>
    /// Test GraphQL endpoint - returns a simple greeting
    /// </summary>
    public string Hello => "Hello from GraphQL! SportyBuddies API is running with GraphQL support.";

    /// <summary>
    /// Gets the current user's profile
    /// </summary>
    [Authorize]
    public async Task<ProfileDto> GetCurrentProfile([Service] ISender sender, CancellationToken cancellationToken)
    {
        var query = new GetCurrentProfileQuery();
        var result = await sender.Send(query, cancellationToken);
        return result.Profile;
    }

    /// <summary>
    /// Gets all profiles (currently returns only the current user's profile)
    /// </summary>
    [Authorize]
    public async Task<IEnumerable<ProfileDto>> GetProfiles([Service] ISender sender, CancellationToken cancellationToken)
    {
        var query = new GetProfilesQuery();
        var result = await sender.Send(query, cancellationToken);
        return result.Profiles;
    }

    /// <summary>
    /// Gets a profile by its ID
    /// </summary>
    [Authorize]
    public async Task<ProfileDto> GetProfileById(
        Guid profileId, 
        [Service] ISender sender, 
        CancellationToken cancellationToken)
    {
        var query = new GetProfileByIdQuery(profileId);
        var result = await sender.Send(query, cancellationToken);
        return result.Profile;
    }
}