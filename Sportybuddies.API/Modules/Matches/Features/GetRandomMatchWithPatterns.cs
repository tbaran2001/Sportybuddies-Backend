using Sportybuddies.API.Common.Patterns;
using Sportybuddies.API.Modules.Matches.Dtos;
using Sportybuddies.API.Services.Domain;

namespace Sportybuddies.API.Modules.Matches.Features;

public record GetRandomMatchWithPatternsQuery(Guid ProfileId) : IRequest<Result<MatchDto>>;

public class GetRandomMatchWithPatternsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/matches/random-enhanced/{profileId:guid}",
                async (Guid profileId, ISender sender) =>
                {
                    var query = new GetRandomMatchWithPatternsQuery(profileId);
                    var result = await sender.Send(query);

                    return result.IsSuccess 
                        ? Results.Ok(result.Value) 
                        : Results.BadRequest(result.Error);
                })
            .RequireAuthorization()
            .WithTags("Matches")
            .WithName("GetRandomMatchWithPatterns")
            .Produces<MatchDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get a random match using enhanced patterns")
            .WithDescription("Get a random match demonstrating Result pattern, Strategy pattern, and Domain Service pattern");
    }
}

internal class GetRandomMatchWithPatternsQueryHandler(
    IMatchFilteringDomainService matchFilteringDomainService)
    : IRequestHandler<GetRandomMatchWithPatternsQuery, Result<MatchDto>>
{
    public async Task<Result<MatchDto>> Handle(GetRandomMatchWithPatternsQuery query, CancellationToken cancellationToken)
    {
        return await matchFilteringDomainService.GetRandomValidMatchAsync(query.ProfileId);
    }
}