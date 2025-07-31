namespace Sportybuddies.API.Modules.Profiles.Features.Commands;

public record UpdateProfileCommand(Guid ProfileId, string Name, string Description, Gender Gender, DateTimeOffset DateOfBirth)
    : ICommand<UpdateProfileResult>;

public record UpdateProfileResult(Guid Id);

public record UpdateProfileRequestDto(string Name, string Description, Gender Gender, DateTimeOffset DateOfBirth);

public class UpdateProfileEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/profiles/{profileId:guid}",
                async (Guid profileId, UpdateProfileRequestDto request, ISender sender) =>
                {
                    var command = request.Adapt<UpdateProfileCommand>() with { ProfileId = profileId };

                    await sender.Send(command);

                    return Results.NoContent();
                })
            .WithTags("Profiles")
            .WithName("UpdateProfile")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update a profile")
            .WithDescription("Update a profile of the current user");
    }
}

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
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

internal class UpdateProfileCommandHandler(
    IProfilesRepository profilesRepository)
    : ICommandHandler<UpdateProfileCommand, UpdateProfileResult>
{
    public async Task<UpdateProfileResult> Handle(UpdateProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = await profilesRepository.GetProfileByIdAsync(command.ProfileId, cancellationToken);
        if (profile is null)
            throw new ProfileNotFoundException(command.ProfileId);

        profile.Update(command.Name, command.Description, command.DateOfBirth, command.Gender);

        return new UpdateProfileResult(profile.Id);
    }
}