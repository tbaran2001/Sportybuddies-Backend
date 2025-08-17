namespace Sportybuddies.API.Modules.Conversations.Features;

public record GetLatestByProfileQuery(Guid ProfileId) : IQuery<GetLatestByProfileResult>;

public record GetLatestByProfileResult(ConversationDto Conversation);

public record GetLatestByProfileResponseDto(ConversationDto Conversation);

public class GetLatestByProfileEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/conversations/GetLatestByProfile/{profileId:guid}",
                async (Guid profileId, ISender sender) =>
                {
                    var query = new GetLatestByProfileQuery(profileId);

                    var result = await sender.Send(query);

                    var response = result.Adapt<GetLatestByProfileResponseDto>();

                    return Results.Ok(response);
                })
            .RequireAuthorization()
            .WithTags("Conversations")
            .WithName("GetLatestByProfile")
            .Produces<GetLatestByProfileResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get latest conversation for a profile")
            .WithDescription("Returns the most recently active conversation for the given profile.");
    }
}

public class GetLatestByProfileQueryValidator : AbstractValidator<GetLatestByProfileQuery>
{
    public GetLatestByProfileQueryValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
    }
}

internal class GetLatestByProfileQueryHandler(
    IConversationsRepository conversationsRepository,
    IProfilesRepository profilesRepository,
    ICurrentUserProvider currentUserProvider)
    : IQueryHandler<GetLatestByProfileQuery, GetLatestByProfileResult>
{
    public async Task<GetLatestByProfileResult> Handle(GetLatestByProfileQuery query,
        CancellationToken cancellationToken)
    {
        var profile = await profilesRepository.GetProfileByIdAsync(query.ProfileId, cancellationToken);
        if (profile is null)
            throw new ProfileNotFoundException(query.ProfileId);

        var currentUserId = currentUserProvider.GetCurrentUserId();
        if (profile.UserId != currentUserId)
            throw new ForbiddenException("You are not allowed to access conversations for this profile.");

        var latestConversation = await conversationsRepository.GetLatestConversationByProfileIdAsync(query.ProfileId);

        if (latestConversation == null)
            throw new NotFoundException($"No conversations found for profile {query.ProfileId}.");

        var conversationDto = latestConversation.Adapt<ConversationDto>();

        return new GetLatestByProfileResult(conversationDto);
    }
}