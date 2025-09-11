using Sportybuddies.API.Common.Patterns;
using Sportybuddies.API.Modules.Profiles.Specifications;

namespace Sportybuddies.API.Modules.Profiles.Features.Commands;

public record UpdateProfileWithPatternsCommand(
    Guid ProfileId,
    string Name,
    string Description,
    Gender Gender,
    DateTimeOffset DateOfBirth)
    : ICommand<Result<UpdateProfileResult>>, IRequireAuthorization
{
    public Guid GetResourceOwnerId()
    {
        // This will be validated by looking up the profile and checking the user ID
        // For now, we'll handle this in the authorization service
        return ProfileId;
    }
}

public record UpdateProfileWithPatternsRequestDto(string Name, string Description, Gender Gender, DateTimeOffset DateOfBirth);

public class UpdateProfileWithPatternsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/profiles/{profileId:guid}/enhanced",
                async (Guid profileId, UpdateProfileWithPatternsRequestDto request, ISender sender) =>
                {
                    var command = new UpdateProfileWithPatternsCommand(
                        profileId, 
                        request.Name, 
                        request.Description, 
                        request.Gender, 
                        request.DateOfBirth);

                    var result = await sender.Send(command);

                    return result.IsSuccess 
                        ? Results.Ok(result.Value) 
                        : Results.BadRequest(result.Error);
                })
            .RequireAuthorization()
            .WithTags("Profiles")
            .WithName("UpdateProfileWithPatterns")
            .Produces<UpdateProfileResult>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update a profile using enhanced patterns")
            .WithDescription("Update a profile demonstrating Result pattern, Specification pattern, and Authorization pattern");
    }
}

public class UpdateProfileWithPatternsCommandValidator : AbstractValidator<UpdateProfileWithPatternsCommand>
{
    public UpdateProfileWithPatternsCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);
        RuleFor(x => x.Gender)
            .IsInEnum();
        RuleFor(x => x.DateOfBirth)
            .Must(dateOfBirth => DateTime.Now.Year - dateOfBirth.Year >= 18);
    }
}

internal class UpdateProfileWithPatternsCommandHandler(
    IProfilesRepository profilesRepository,
    ICurrentUserProvider currentUserProvider)
    : ICommandHandler<UpdateProfileWithPatternsCommand, Result<UpdateProfileResult>>
{
    public async Task<Result<UpdateProfileResult>> Handle(UpdateProfileWithPatternsCommand command, CancellationToken cancellationToken)
    {
        // Use specification pattern to get profile
        var profileSpec = new ProfileByIdSpecification(command.ProfileId);
        var profile = await profilesRepository.GetProfileBySpecificationAsync(profileSpec, cancellationToken);
        
        if (profile is null)
            return Result.Failure<UpdateProfileResult>($"Profile with ID {command.ProfileId} not found");

        // Authorization is handled by the AuthorizationBehavior, but we need to implement the 
        // GetResourceOwnerId properly for complex scenarios
        var currentUserId = currentUserProvider.GetCurrentUserId();
        if (profile.UserId != currentUserId)
            return Result.Failure<UpdateProfileResult>("You are not allowed to modify this profile.");

        // Update the profile
        profile.Update(command.Name, command.Description, command.DateOfBirth, command.Gender);

        // Return success result
        return Result.Success(new UpdateProfileResult(profile.Id));
    }
}