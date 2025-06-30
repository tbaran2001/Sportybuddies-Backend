namespace Sportybuddies.API.Modules.Profiles.Dtos;

public record ProfileDto(
    Guid Id,
    string Name,
    string Description,
    string MainPhotoUrl,
    DateTimeOffset CreatedOn,
    Gender Gender,
    DateTimeOffset DateOfBirth,
    Preferences Preferences,
    Location Location,
    List<SportDto> Sports);