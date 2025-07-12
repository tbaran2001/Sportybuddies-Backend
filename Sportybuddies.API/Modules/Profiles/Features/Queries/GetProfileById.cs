namespace Sportybuddies.API.Modules.Profiles.Features.Queries;

public record GetProfileByIdQuery(Guid ProfileId) : IQuery<GetProfileByIdResult>;

public record GetProfileByIdResult(ProfileDto Profile);

public record GetProfileByIdResponseDto(ProfileDto Profile);

public class GetProfileByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/profiles/{profileId:guid}", async (Guid profileId, ISender sender) =>
            {
                var query = new GetProfileByIdQuery(profileId);

                var result = await sender.Send(query);

                var response = result.Adapt<GetProfileByIdResponseDto>();

                return Results.Ok(response);
            })
            .RequireAuthorization()
            .WithName("GetProfileById")
            .Produces<GetProfileByIdResponseDto>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get a profile by id")
            .WithDescription("Get a profile by id");
    }
}

public class GetProfileByIdQueryValidator : AbstractValidator<GetProfileByIdQuery>
{
    public GetProfileByIdQueryValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
    }
}

internal class GetProfileByIdQueryHandler(
    IProfilesRepository profilesRepository)
    : IQueryHandler<GetProfileByIdQuery, GetProfileByIdResult>
{
    public async Task<GetProfileByIdResult> Handle(GetProfileByIdQuery query, CancellationToken cancellationToken)
    {
        var profile = await profilesRepository.GetProfileByIdWithSportsAsync(query.ProfileId, cancellationToken);
        if (profile == null)
            throw new ProfileNotFoundException(query.ProfileId);

        var profileDto = profile.Adapt<ProfileDto>();

        return new GetProfileByIdResult(profileDto);
    }
}