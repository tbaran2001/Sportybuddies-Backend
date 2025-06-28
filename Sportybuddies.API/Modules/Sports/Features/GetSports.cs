namespace Sportybuddies.API.Modules.Sports.Features;

public record GetSportsQuery : IQuery<GetSportsResult>;

public record GetSportsResult(IEnumerable<SportDto> Sports);

public record GetSportsResponseDto(IEnumerable<SportDto> Sports);

public class GetSportsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/sports", async (ISender sender) =>
            {
                var query = new GetSportsQuery();

                var result = await sender.Send(query);

                var response = result.Adapt<GetSportsResponseDto>();

                return Results.Ok(response);
            })
            .RequireAuthorization()
            .WithTags("Sports")
            .WithName("GetSports")
            .Produces<GetSportsResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get all sports")
            .WithDescription("Get all sports");
    }
}

internal class GetSportsQueryHandler(ISportsRepository sportsRepository)
    : IQueryHandler<GetSportsQuery, GetSportsResult>
{
    public async Task<GetSportsResult> Handle(GetSportsQuery query, CancellationToken cancellationToken)
    {
        var sports = await sportsRepository.GetAllSportsAsync();

        var sportDtos = sports.Adapt<IEnumerable<SportDto>>();

        return new GetSportsResult(sportDtos);
    }
}