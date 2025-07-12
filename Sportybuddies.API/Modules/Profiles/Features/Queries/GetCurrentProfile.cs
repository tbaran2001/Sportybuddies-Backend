using Sportybuddies.API.Common.Web;

namespace Sportybuddies.API.Modules.Profiles.Features.Queries;

public record GetCurrentProfileQuery : IQuery<GetCurrentProfileResult>;

public record GetCurrentProfileResult(ProfileDto Profile);

public record GetCurrentProfileResponseDto(ProfileDto Profile);

public class GetCurrentProfileEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/profiles/me", async (ISender sender) =>
            {
                var query = new GetCurrentProfileQuery();

                var result = await sender.Send(query);

                var response = result.Adapt<GetCurrentProfileResponseDto>();

                return Results.Ok(response);
            })
            .RequireAuthorization()
            .WithTags("Profiles")
            .WithName("GetCurrentProfile")
            .Produces<GetCurrentProfileResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get current user profile")
            .WithDescription("Get the profile of the current user");
    }
}

internal class GetCurrentProfileQueryHandler(
    IProfilesRepository profilesRepository,
    ICurrentUserProvider currentUserProvider) : IQueryHandler<GetCurrentProfileQuery, GetCurrentProfileResult>
{
    public async Task<GetCurrentProfileResult> Handle(GetCurrentProfileQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserProvider.GetCurrentUserId();

        var profile = await profilesRepository.GetProfileByUserIdWithSportsAsync(currentUserId, cancellationToken);
        if (profile == null)
            throw new ProfileNotFoundException(currentUserId);

        var profileDto = profile.Adapt<ProfileDto>();

        return new GetCurrentProfileResult(profileDto);
    }
}