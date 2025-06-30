namespace Sportybuddies.API.Modules.Sports.Features;

public record GetSportByIdQuery(Guid SportId) : IQuery<GetSportByIdResult>;

public record GetSportByIdResult(SportDto Sport);

public record GetSportByIdResponseDto(SportDto Sport);

public class GetSportById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/sports/{sportId:guid}", async (Guid sportId, ISender sender) =>
            {
                var query = new GetSportByIdQuery(sportId);

                var result = await sender.Send(query);

                var response = result.Adapt<GetSportByIdResponseDto>();

                return Results.Ok(response);
            })
            .RequireAuthorization()
            .WithName("GetSportById")
            .Produces<GetSportByIdResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get a sport by id")
            .WithDescription("Get a sport by id");
    }
}

internal class GetSportByIdQueryHandler(ISportsRepository sportsRepository)
    : IQueryHandler<GetSportByIdQuery, GetSportByIdResult>
{
    public async Task<GetSportByIdResult> Handle(GetSportByIdQuery query, CancellationToken cancellationToken)
    {
        var sport = await sportsRepository.GetSportByIdAsync(query.SportId, cancellationToken);
        if (sport is null)
            throw new SportNotFoundException(query.SportId);

        var sportDto = sport.Adapt<SportDto>();

        return new GetSportByIdResult(sportDto);
    }
}