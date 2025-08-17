using Sportybuddies.API.Common.Web;

namespace Sportybuddies.API.Modules.Matches.Features;

public record UpdateMatchCommand(Guid MatchId, Swipe Swipe) : ICommand<UpdateMatchResult>;

public record UpdateMatchResult(Guid Id);

public record UpdateMatchRequestDto(Swipe Swipe);

public class UpdateMatchEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/matches/{matchId:guid}", async (Guid matchId, UpdateMatchRequestDto request, ISender sender) =>
            {
                var command = new UpdateMatchCommand(matchId, request.Swipe);

                await sender.Send(command);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithTags("Matches")
            .WithName("UpdateMatch")
            .Produces<UpdateMatchResult>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update a match")
            .WithDescription("Update a match by match id");
    }
}

public class UpdateMatchCommandValidator : AbstractValidator<UpdateMatchCommand>
{
    public UpdateMatchCommandValidator()
    {
        RuleFor(x => x.MatchId).NotEmpty();
        RuleFor(x => x.Swipe).IsInEnum();
    }
}

internal class UpdateMatchCommandHandler(
    IMatchesRepository matchesRepository,
    IBuddyService buddyService,
    IProfilesRepository profilesRepository,
    ICurrentUserProvider currentUserProvider)
    : ICommandHandler<UpdateMatchCommand, UpdateMatchResult>
{
    public async Task<UpdateMatchResult> Handle(UpdateMatchCommand command, CancellationToken cancellationToken)
    {
        var match = await matchesRepository.GetMatchByIdAsync(command.MatchId);
        if (match is null)
            throw new MatchNotFoundException(command.MatchId);

        var profile = await profilesRepository.GetProfileByIdAsync(match.ProfileId, cancellationToken);
        if (profile is null)
            throw new ProfileNotFoundException(match.ProfileId);

        var currentUserId = currentUserProvider.GetCurrentUserId();
        if (profile.UserId != currentUserId)
            throw new ForbiddenException("You are not allowed to modify this match.");

        var oppositeMatch = await matchesRepository.GetMatchByIdAsync(match.OppositeMatchId);
        if (oppositeMatch is null)
            throw new MatchNotFoundException(match.OppositeMatchId);

        match.SetSwipe(command.Swipe, oppositeMatch.Swipe);

        await buddyService.AddBuddyAsync(match, oppositeMatch);

        return new UpdateMatchResult(match.Id);
    }
}