namespace Sportybuddies.API.Modules.Profiles.Features.Queries;

public record GetProfilesQuery : IQuery<GetProfilesResult>;

public record GetProfilesResult(IEnumerable<ProfileDto> Profiles);

public record GetProfilesResponseDto(IEnumerable<ProfileDto> Profiles);

public class GetProfileEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/profiles", async (ISender sender) =>
            {
                var query = new GetProfilesQuery();

                var result = await sender.Send(query);

                var response = result.Adapt<GetProfilesResponseDto>();

                return Results.Ok(response);
            })
            .WithName("GetProfiles")
            .Produces<GetProfilesResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get profiles")
            .WithDescription("Get profiles");
    }
}

internal class GetProfilesQueryHandler(IProfilesRepository profilesRepository)
    : IQueryHandler<GetProfilesQuery, GetProfilesResult>
{
    public async Task<GetProfilesResult> Handle(GetProfilesQuery query, CancellationToken cancellationToken)
    {
        var profiles = await profilesRepository.GetAllProfilesWthSportsAsync(cancellationToken);

        var profileDtos = profiles.Adapt<IEnumerable<ProfileDto>>();

        return new GetProfilesResult(profileDtos);
    }
}