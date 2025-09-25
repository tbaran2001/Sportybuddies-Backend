#nullable enable
using HotChocolate;
using HotChocolate.Authorization;
using Sportybuddies.API.Modules.Profiles.Features.Queries;

namespace Sportybuddies.API.Modules.Profiles.GraphQL;

/// <summary>
/// Input type for updating a profile
/// </summary>
public record UpdateProfileInput(
    Guid ProfileId,
    string? Name,
    string? Description,
    DateTimeOffset? DateOfBirth,
    Gender? Gender);

/// <summary>
/// Input type for adding a sport to a profile
/// </summary>
public record AddProfileSportInput(
    Guid ProfileId,
    Guid SportId);

/// <summary>
/// Input type for removing a sport from a profile  
/// </summary>
public record RemoveProfileSportInput(
    Guid ProfileId,
    Guid SportId);

/// <summary>
/// Input type for updating profile location
/// </summary>
public record UpdateProfileLocationInput(
    Guid ProfileId,
    double Latitude,
    double Longitude,
    string? Address);

/// <summary>
/// GraphQL mutations for the Profiles module
/// </summary>
public class ProfileMutations
{
    /// <summary>
    /// Update a profile with partial information
    /// </summary>
    /// <param name="input">Update profile input</param>
    /// <param name="sender">MediatR sender</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated profile</returns>
    [Authorize]
    public async Task<ProfileDto?> UpdateProfile(
        UpdateProfileInput input,
        [Service] ISender sender,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateProfilePartialCommand(
            input.ProfileId,
            input.Name,
            input.Description,
            input.Gender,
            input.DateOfBirth);
            
        await sender.Send(command, cancellationToken);
        
        // Return updated profile
        var query = new GetProfileByIdQuery(input.ProfileId);
        var result = await sender.Send(query, cancellationToken);
        return result.Profile;
    }
    
    /// <summary>
    /// Add a sport to a user's profile
    /// </summary>
    /// <param name="input">Add sport input</param>
    /// <param name="sender">MediatR sender</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated profile</returns>
    [Authorize]
    public async Task<ProfileDto?> AddProfileSport(
        AddProfileSportInput input,
        [Service] ISender sender,
        CancellationToken cancellationToken = default)
    {
        var command = new AddProfileSportCommand(input.ProfileId, input.SportId);
        await sender.Send(command, cancellationToken);
        
        // Return updated profile
        var query = new GetProfileByIdQuery(input.ProfileId);
        var result = await sender.Send(query, cancellationToken);
        return result.Profile;
    }
    
    /// <summary>
    /// Remove a sport from a user's profile
    /// </summary>
    /// <param name="input">Remove sport input</param>
    /// <param name="sender">MediatR sender</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated profile</returns>
    [Authorize]
    public async Task<ProfileDto?> RemoveProfileSport(
        RemoveProfileSportInput input,
        [Service] ISender sender,
        CancellationToken cancellationToken = default)
    {
        var command = new RemoveProfileSportCommand(input.ProfileId, input.SportId);
        await sender.Send(command, cancellationToken);
        
        // Return updated profile
        var query = new GetProfileByIdQuery(input.ProfileId);
        var result = await sender.Send(query, cancellationToken);
        return result.Profile;
    }
    
    /// <summary>
    /// Update a user's profile location
    /// </summary>
    /// <param name="input">Update location input</param>
    /// <param name="sender">MediatR sender</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated profile</returns>
    [Authorize]
    public async Task<ProfileDto?> UpdateProfileLocation(
        UpdateProfileLocationInput input,
        [Service] ISender sender,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateProfileLocationCommand(
            input.ProfileId, 
            input.Latitude, 
            input.Longitude, 
            input.Address ?? string.Empty);
        await sender.Send(command, cancellationToken);
        
        // Return updated profile
        var query = new GetProfileByIdQuery(input.ProfileId);
        var result = await sender.Send(query, cancellationToken);
        return result.Profile;
    }
}