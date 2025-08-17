namespace Sportybuddies.API.Modules.Conversations.Features;

public record GetConversationMessagesQuery(Guid ConversationId) : IQuery<GetConversationMessagesResult>;

public record GetConversationMessagesResult(IEnumerable<MessageDto> Messages);

public record GetConversationMessagesResponseDto(IEnumerable<MessageDto> Messages);

public class GetConversationMessagesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/conversations/{conversationId:guid}/messages",
                async (Guid conversationId, ISender sender) =>
                {
                    var query = new GetConversationMessagesQuery(conversationId);

                    var result = await sender.Send(query);

                    var response = result.Adapt<GetConversationMessagesResponseDto>();

                    return Results.Ok(response);
                })
            .RequireAuthorization()
            .WithTags("Conversations")
            .WithName("GetConversationMessages")
            .Produces<GetConversationMessagesResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get messages for a conversation")
            .WithDescription("Get all messages for a conversation by conversation id");
    }
}

public class GetConversationMessagesQueryValidator : AbstractValidator<GetConversationMessagesQuery>
{
    public GetConversationMessagesQueryValidator()
    {
        RuleFor(x => x.ConversationId).NotEmpty();
    }
}

internal class GetConversationMessagesQueryHandler(
    IConversationsRepository conversationsRepository,
    IProfilesRepository profilesRepository,
    ICurrentUserProvider currentUserProvider)
    : IQueryHandler<GetConversationMessagesQuery, GetConversationMessagesResult>
{
    public async Task<GetConversationMessagesResult> Handle(GetConversationMessagesQuery query,
        CancellationToken cancellationToken)
    {
        var conversationWithParticipants = await conversationsRepository.GetConversationByIdAsync(query.ConversationId);
        if (conversationWithParticipants is null)
            throw new ConversationNotFoundException(query.ConversationId);

        var currentUserId = currentUserProvider.GetCurrentUserId();
        var currentProfile =
            await profilesRepository.GetProfileByUserIdWithSportsAsync(currentUserId, cancellationToken);
        if (currentProfile is null)
            throw new ProfileNotFoundException(currentUserId);

        if (conversationWithParticipants.Participants.All(p => p.ProfileId != currentProfile.Id))
            throw new ForbiddenException("You are not allowed to access messages for this conversation.");

        var conversation = await conversationsRepository.GetConversationByIdWithMessagesAsync(query.ConversationId);
        if (conversation is null)
            throw new ConversationNotFoundException(query.ConversationId);

        var messageDtos = conversation.Messages.Adapt<IEnumerable<MessageDto>>();

        return new GetConversationMessagesResult(messageDtos);
    }
}