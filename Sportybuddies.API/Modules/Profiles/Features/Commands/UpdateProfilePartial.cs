namespace Sportybuddies.API.Modules.Profiles.Features.Commands;

public record UpdateProfilePartialRequestDto(
    string Name,
    string Description,
    DateTimeOffset? DateOfBirth,
    Gender? Gender);

public record UpdateProfilePartialCommand(
    Guid ProfileId,
    string Name,
    string Description,
    Gender? Gender,
    DateTimeOffset? DateOfBirth) : ICommand<UpdateProfileResult>;

public class UpdateProfilePartialEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("api/profiles/{profileId:guid}",
                async (Guid profileId, UpdateProfilePartialRequestDto request, ISender sender) =>
                {
                    var command = new UpdateProfilePartialCommand(profileId, request.Name, request.Description,
                        request.Gender, request.DateOfBirth);
                    await sender.Send(command);
                    return Results.NoContent();
                })
            .RequireAuthorization()
            .WithTags("Profiles")
            .WithName("UpdateProfilePartial")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Partially update a profile")
            .WithDescription("Partially update a profile of the current user");
    }
}

public class UpdateProfilePartialCommandValidator : AbstractValidator<UpdateProfilePartialCommand>
{
    public UpdateProfilePartialCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50)
            .When(x => x.Name != null);
        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => x.Description != null);
        RuleFor(x => x.Gender)
            .IsInEnum()
            .When(x => x.Gender != null);
        RuleFor(x => x.DateOfBirth)
            .Must(dateOfBirth => dateOfBirth == null || DateTime.Now.Year - dateOfBirth.Value.Year >= 18)
            .When(x => x.DateOfBirth != null);
    }
}

internal class UpdateProfilePartialCommandHandler(
    IProfilesRepository profilesRepository,
    ICurrentUserProvider currentUserProvider)
    : ICommandHandler<UpdateProfilePartialCommand, UpdateProfileResult>
{
    public async Task<UpdateProfileResult> Handle(UpdateProfilePartialCommand command,
        CancellationToken cancellationToken)
    {
        var profile = await profilesRepository.GetProfileByIdAsync(command.ProfileId, cancellationToken);
        if (profile is null)
            throw new ProfileNotFoundException(command.ProfileId);

        var currentUserId = currentUserProvider.GetCurrentUserId();
        if (profile.UserId != currentUserId)
            throw new ForbiddenException("You are not allowed to modify this profile.");

        profile.UpdatePartial(command.Name, command.Description, command.DateOfBirth, command.Gender);

        return new UpdateProfileResult(profile.Id);
    }
}