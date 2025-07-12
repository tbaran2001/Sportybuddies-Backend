namespace Sportybuddies.API.Modules.Profiles.Features.Commands;

public record UpdateProfilePreferencesCommand(
    Guid ProfileId,
    int MinAge,
    int MaxAge,
    int MaxDistance,
    Gender PreferredGender)
    : ICommand;

public record UpdateProfilePreferencesRequestDto(int MinAge, int MaxAge, int MaxDistance, Gender PreferredGender);

public class UpdateProfilePreferencesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/profiles/{profileId:guid}/preferences",
                async (Guid profileId, UpdateProfilePreferencesRequestDto request, ISender sender) =>
                {
                    var command = request.Adapt<UpdateProfilePreferencesCommand>() with { ProfileId = profileId };

                    await sender.Send(command);

                    return Results.NoContent();
                })
            .RequireAuthorization()
            .WithName("UpdateProfilePreferences")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update profile preferences")
            .WithDescription("Update profile preferences of the current user");
    }
}

public class UpdateProfilePreferencesCommandValidator : AbstractValidator<UpdateProfilePreferencesCommand>
{
    public UpdateProfilePreferencesCommandValidator()
    {
        RuleFor(x => x.MinAge)
            .InclusiveBetween(18, 120)
            .LessThanOrEqualTo(x => x.MaxAge);
        RuleFor(x => x.MaxAge)
            .InclusiveBetween(18, 120)
            .GreaterThanOrEqualTo(x => x.MinAge);
        RuleFor(x => x.MaxDistance)
            .InclusiveBetween(1, 100);
        RuleFor(x => x.PreferredGender)
            .IsInEnum();
    }
}

internal class UpdateProfilePreferencesCommandHandler(
    IProfilesRepository profilesRepository) : ICommandHandler<UpdateProfilePreferencesCommand>
{
    public async Task<Unit> Handle(UpdateProfilePreferencesCommand command, CancellationToken cancellationToken)
    {
        var profile = await profilesRepository.GetProfileByIdAsync(command.ProfileId, cancellationToken);
        if (profile == null)
            throw new ProfileNotFoundException(command.ProfileId);

        profile.UpdatePreferences(Preferences.Create(command.MinAge, command.MaxAge, command.MaxDistance,
            command.PreferredGender));

        return Unit.Value;
    }
}