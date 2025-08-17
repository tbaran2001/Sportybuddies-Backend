namespace Sportybuddies.API.Modules.Matches.Features;

public record GetProfileMatchesQuery(Guid ProfileId) : IQuery<GetProfileMatchesResult>;

public record GetProfileMatchesResult(IEnumerable<MatchDto> Matches);

public record GetProfileMatchesResponseDto(IEnumerable<MatchDto> Matches);

public class GetProfileMatchesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/matches/GetMatchesForProfile/{profileId:guid}", async (Guid profileId, ISender sender) =>
            {
                var query = new GetProfileMatchesQuery(profileId);

                var result = await sender.Send(query);

                var response = result.Adapt<GetProfileMatchesResponseDto>();

                return Results.Ok(response);
            })
            .RequireAuthorization()
            .WithTags("Matches")
            .WithName("GetProfileMatches")
            .Produces<GetProfileMatchesResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get matches for a profile")
            .WithDescription("Get all matches for a profile by profile id");
    }
}

internal class GetProfileMatchesQueryHandler(
    IMatchesRepository matchesRepository,
    IProfilesRepository profilesRepository,
    ICurrentUserProvider currentUserProvider)
    : IQueryHandler<GetProfileMatchesQuery, GetProfileMatchesResult>
{
    public async Task<GetProfileMatchesResult> Handle(GetProfileMatchesQuery query, CancellationToken cancellationToken)
    {
        var profile = await profilesRepository.GetProfileByIdAsync(query.ProfileId, cancellationToken);
        if (profile is null)
            throw new ProfileNotFoundException(query.ProfileId);

        var currentUserId = currentUserProvider.GetCurrentUserId();
        if (profile.UserId != currentUserId)
            throw new ForbiddenException("You are not allowed to access matches for this profile.");

        var matches = await matchesRepository.GetProfileMatchesAsync(query.ProfileId);
        if (matches is null)
            throw new MatchesNotFoundException(query.ProfileId);

        var matchDtos = matches.Adapt<IEnumerable<MatchDto>>();

        return new GetProfileMatchesResult(matchDtos);
    }
}