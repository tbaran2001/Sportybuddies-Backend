#nullable enable
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Data;
using Sportybuddies.API.Modules.Profiles.Features.Queries;

namespace Sportybuddies.API.Modules.Profiles.GraphQL;

/// <summary>
/// GraphQL queries for the Profiles module
/// </summary>
[Authorize]
public class ProfileQueries
{
    /// <summary>
    /// Get all profiles (returns current user's profile)
    /// </summary>
    /// <param name="sender">MediatR sender</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of profiles</returns>
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<ProfileDto>> GetProfiles(
        [Service] ISender sender,
        CancellationToken cancellationToken = default)
    {
        var query = new GetProfilesQuery();
        var result = await sender.Send(query, cancellationToken);
        return result.Profiles;
    }
    
    /// <summary>
    /// Get a specific profile by ID
    /// </summary>
    /// <param name="profileId">The profile ID</param>
    /// <param name="sender">MediatR sender</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The profile</returns>
    public async Task<ProfileDto?> GetProfile(
        Guid profileId,
        [Service] ISender sender,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetProfileByIdQuery(profileId);
            var result = await sender.Send(query, cancellationToken);
            return result.Profile;
        }
        catch (ProfileNotFoundException)
        {
            return null;
        }
    }
    
    /// <summary>
    /// Get the current user's profile
    /// </summary>
    /// <param name="sender">MediatR sender</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The current user's profile</returns>
    public async Task<ProfileDto?> GetCurrentProfile(
        [Service] ISender sender,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetCurrentProfileQuery();
            var result = await sender.Send(query, cancellationToken);
            return result.Profile;
        }
        catch (ProfileNotFoundException)
        {
            return null;
        }
    }
}