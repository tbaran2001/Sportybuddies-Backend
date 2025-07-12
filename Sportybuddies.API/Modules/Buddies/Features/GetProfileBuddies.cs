using Sportybuddies.API.Modules.Buddies.Dtos;

namespace Sportybuddies.API.Modules.Buddies.Features;

public record GetProfileBuddiesQuery(Guid ProfileId) : IQuery<GetProfileBuddiesResult>;

public record GetProfileBuddiesResult(IEnumerable<BuddyDto> Buddies);

public record GetProfileBuddiesResponseDto(IEnumerable<BuddyDto> Buddies);

public class GetProfileBuddiesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/buddies/GetProfileBuddies/{profileId:guid}", async (Guid profileId, ISender sender) =>
            {
                var query = new GetProfileBuddiesQuery(profileId);

                var result = await sender.Send(query);

                var response = result.Adapt<GetProfileBuddiesResponseDto>();

                return Results.Ok(response);
            })
            .RequireAuthorization()
            .WithTags("Buddies")
            .WithName("GetProfileBuddies")
            .Produces<GetProfileBuddiesResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get profile buddies")
            .WithDescription("Get profile buddies of the current user");
    }
}

internal class GetProfileBuddiesQueryHandler(
    IBuddiesRepository buddiesRepository)
    : IQueryHandler<GetProfileBuddiesQuery, GetProfileBuddiesResult>
{
    public async Task<GetProfileBuddiesResult> Handle(GetProfileBuddiesQuery query, CancellationToken cancellationToken)
    {
        var buddies = await buddiesRepository.GetProfileBuddiesAsync(query.ProfileId);

        var buddyDtos = buddies.Adapt<IEnumerable<BuddyDto>>();

        return new GetProfileBuddiesResult(buddyDtos);
    }
}