namespace Sportybuddies.API.Modules.Conversations.Features;

public record CreateConversationCommand(Guid ProfileId, Guid ParticipantId) : ICommand<CreateConversationResult>;

public record CreateConversationResult(ConversationDto Conversation);

public record CreateConversationRequestDto(Guid ProfileId, Guid ParticipantId);

public record CreateConversationResponseDto(ConversationDto Conversation);

public class CreateConversationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/conversations", async (CreateConversationRequestDto request, ISender sender) =>
            {
                var command = request.Adapt<CreateConversationCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateConversationResponseDto>();

                return Results.CreatedAtRoute("GetConversationById", new { conversationId = response.Conversation.Id },
                    response);
            })
            .RequireAuthorization()
            .WithTags("Conversations")
            .WithName("CreateConversation")
            .Produces<CreateConversationResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create a conversation")
            .WithDescription("Create a conversation");
    }
}

public class CreateConversationCommandValidator : AbstractValidator<CreateConversationCommand>
{
    public CreateConversationCommandValidator()
    {
        RuleFor(x => x.ParticipantId).NotEmpty();
    }
}

internal class CreateConversationCommandHandler(
    IConversationService conversationService)
    : ICommandHandler<CreateConversationCommand, CreateConversationResult>
{
    public async Task<CreateConversationResult> Handle(CreateConversationCommand command,
        CancellationToken cancellationToken)
    {
        var conversation = await conversationService.CreateConversationAsync(command.ProfileId, command.ParticipantId);

        var conversationDto = conversation.Adapt<ConversationDto>();

        return new CreateConversationResult(conversationDto);
    }
}