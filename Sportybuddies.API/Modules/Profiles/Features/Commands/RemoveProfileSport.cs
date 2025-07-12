using Sportybuddies.API.Modules.Profiles.Exceptions.Application;

namespace Sportybuddies.API.Modules.Profiles.Features.Commands;

public record RemoveProfileSportCommand(Guid ProfileId, Guid SportId) : ICommand;

public record ProfileSportRemovedDomainEvent(Guid ProfileId, Guid SportId) : IDomainEvent;

public record RemoveProfileSportRequestDto(Guid SportId);

public class RemoveProfileSportEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/profiles/{profileId:guid}/RemoveProfileSport",
                async (Guid profileId, RemoveProfileSportRequestDto request, ISender sender) =>
                {
                    var command = new RemoveProfileSportCommand(profileId, request.SportId);

                    await sender.Send(command);

                    return Results.NoContent();
                })
            .RequireAuthorization()
            .WithName("RemoveProfileSport")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Remove a sport from a profile")
            .WithDescription("Remove a sport from a profile");
    }
}

public class RemoveProfileSportCommandValidator : AbstractValidator<RemoveProfileSportCommand>
{
    public RemoveProfileSportCommandValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
        RuleFor(x => x.SportId).NotEmpty();
    }
}

internal class RemoveProfileSportCommandHandler(
    IProfilesRepository profilesRepository,
    ISportsRepository sportsRepository) : ICommandHandler<RemoveProfileSportCommand>
{
    public async Task<Unit> Handle(RemoveProfileSportCommand command, CancellationToken cancellationToken)
    {
        var profile = await profilesRepository.GetProfileByIdWithSportsAsync(command.ProfileId, cancellationToken);
        if (profile == null)
            throw new ProfileNotFoundException(command.ProfileId);

        var sport = await sportsRepository.GetSportByIdAsync(command.SportId, cancellationToken);
        if (sport == null)
            throw new SportNotFoundException(command.SportId);

        profile.RemoveSport(sport);

        return Unit.Value;
    }
}