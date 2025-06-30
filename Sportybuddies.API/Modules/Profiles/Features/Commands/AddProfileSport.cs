namespace Sportybuddies.API.Modules.Profiles.Features.Commands;

public record AddProfileSportCommand(Guid ProfileId, Guid SportId) : ICommand;

public record ProfileSportAddedDomainEvent(Guid ProfileId, Guid SportId) : IDomainEvent;

public record AddProfileSportRequestDto(Guid SportId);

public class AddProfileSportEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/profiles/{profileId:guid}/AddProfileSport",
                async (Guid profileId, AddProfileSportRequestDto request, ISender sender) =>
                {
                    var command = new AddProfileSportCommand(profileId, request.SportId);

                    await sender.Send(command);

                    return Results.NoContent();
                })
            .RequireAuthorization()
            .WithName("AddProfileSport")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Add a sport to a profile")
            .WithDescription("Add a sport to a profile");
    }
}

public class AddProfileSportCommandValidator : AbstractValidator<AddProfileSportCommand>
{
    public AddProfileSportCommandValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
        RuleFor(x => x.SportId).NotEmpty();
    }
}

internal class AddProfileSportCommandHandler(
    IProfilesRepository profilesRepository,
    ISportsRepository sportsRepository) : ICommandHandler<AddProfileSportCommand>
{
    public async Task<Unit> Handle(AddProfileSportCommand command, CancellationToken cancellationToken)
    {
        var profile = await profilesRepository.GetProfileByIdWithSportsAsync(command.ProfileId, cancellationToken);
        if (profile == null)
            throw new ProfileNotFoundException(command.ProfileId);

        var sport = await sportsRepository.GetSportByIdAsync(command.SportId, cancellationToken);
        if (sport == null)
            throw new SportNotFoundException(command.SportId);

        profile.AddSport(sport);

        return Unit.Value;
    }
}