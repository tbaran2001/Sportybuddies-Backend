namespace Sportybuddies.API.Modules.Conversations.Features;

public record GetProfileConversationsQuery(Guid ProfileId) : IQuery<GetProfileConversationsResult>;

public record GetProfileConversationsResult(IEnumerable<ConversationDto> Conversations);

public record GetProfileConversationsResponseDto(IEnumerable<ConversationDto> Conversations);

public class GetProfileConversationsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/conversations/GetByProfile/{profileId:guid}",
                async (Guid profileId, ISender sender) =>
                {
                    var query = new GetProfileConversationsQuery(profileId);

                    var result = await sender.Send(query);

                    var response = result.Adapt<GetProfileConversationsResponseDto>();

                    return Results.Ok(response);
                })
            .RequireAuthorization()
            .WithTags("Conversations")
            .WithName("GetProfileConversations")
            .Produces<GetProfileConversationsResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get all conversations for a profile")
            .WithDescription("Get all conversations for a specific profile by profile id");
    }
}

public class GetProfileConversationsQueryValidator : AbstractValidator<GetProfileConversationsQuery>
{
    public GetProfileConversationsQueryValidator()
    {
        RuleFor(x => x.ProfileId)
            .NotEmpty();
    }
}

internal class GetProfileConversationsQueryHandler(IConversationsRepository conversationsRepository)
    : IQueryHandler<GetProfileConversationsQuery, GetProfileConversationsResult>
{
    public async Task<GetProfileConversationsResult> Handle(GetProfileConversationsQuery query,
        CancellationToken cancellationToken)
    {
        var conversations = await conversationsRepository.GetConversationsByProfileIdAsync(query.ProfileId);

        var conversationDtos = conversations.Adapt<IEnumerable<ConversationDto>>();

        return new GetProfileConversationsResult(conversationDtos);
    }
}
