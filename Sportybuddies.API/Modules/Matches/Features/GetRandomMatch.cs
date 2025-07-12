namespace Sportybuddies.API.Modules.Matches.Features;

public record GetRandomMatchQuery(Guid ProfileId) : IQuery<GetRandomMatchResult>;

public record GetRandomMatchResult(MatchDto Match);

public record GetRandomMatchResponseDto(MatchDto Match);

public class GetRandomMatchEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/matches/GetRandomMatch/{profileId:guid}", async (Guid profileId, ISender sender) =>
            {
                var query = new GetRandomMatchQuery(profileId);

                var result = await sender.Send(query);

                var response = result.Adapt<GetRandomMatchResponseDto>();

                return Results.Ok(response);
            })
            .RequireAuthorization()
            .WithTags("Matches")
            .WithName("GetRandomMatch")
            .Produces<GetRandomMatchResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get random match for a current profile")
            .WithDescription("Get a random match for a profile by current profile id");
    }
}

internal class GetRandomMatchQueryHandler(
    IMatchService matchService)
    : IQueryHandler<GetRandomMatchQuery, GetRandomMatchResult>
{
    public async Task<GetRandomMatchResult> Handle(GetRandomMatchQuery query, CancellationToken cancellationToken)
    {
        var matchDto = await matchService.GetRandomMatchAsync(query.ProfileId);
        if (matchDto is null)
            throw new RandomMatchNotFoundException();

        return new GetRandomMatchResult(matchDto);
    }
}