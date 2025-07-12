namespace Sportybuddies.API.Modules.Profiles.Features.Commands;

public record UpdateProfileLocationCommand(Guid ProfileId, double Latitude, double Longitude, string Address)
    : ICommand;

public record UpdateProfileLocationRequestDto(double Latitude, double Longitude, string Address);

public class UpdateProfileLocationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/profiles/{profileId:guid}/location",
                async (Guid profileId, UpdateProfileLocationRequestDto request, ISender sender) =>
                {
                    var command = request.Adapt<UpdateProfileLocationCommand>() with { ProfileId = profileId };

                    await sender.Send(command);

                    return Results.NoContent();
                })
            .RequireAuthorization()
            .WithName("UpdateProfileLocation")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update profile location")
            .WithDescription("Update profile location of the current user");
    }
}

public class UpdateProfileLocationCommandValidator : AbstractValidator<UpdateProfileLocationCommand>
{
    public UpdateProfileLocationCommandValidator()
    {
        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90);
        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180);
    }
}

internal class UpdateProfileLocationCommandHandler(
    IProfilesRepository profilesRepository) : ICommandHandler<UpdateProfileLocationCommand>
{
    public async Task<Unit> Handle(UpdateProfileLocationCommand command, CancellationToken cancellationToken)
    {
        var profile = await profilesRepository.GetProfileByIdWithSportsAsync(command.ProfileId, cancellationToken);
        if (profile == null)
            throw new ProfileNotFoundException(command.ProfileId);

        var location = Location.Create(command.Latitude, command.Longitude, command.Address);

        profile.UpdateLocation(location);

        return Unit.Value;
    }
}