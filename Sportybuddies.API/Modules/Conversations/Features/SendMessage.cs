namespace Sportybuddies.API.Modules.Conversations.Features;

public record SendMessageCommand(Guid ProfileId, Guid ConversationId, string Content) : ICommand<SendMessageResult>;

public record SendMessageResult(MessageDto Message);

public record SendMessageRequestDto(Guid ProfileId, string Content);

public record SendMessageResponseDto(MessageDto Message);

public class SendMessageEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/conversations/{conversationId:guid}/messages",
                async (Guid conversationId, SendMessageRequestDto request, ISender sender) =>
                {
                    var command = new SendMessageCommand(request.ProfileId, conversationId, request.Content);

                    var result = await sender.Send(command);

                    var response = result.Adapt<SendMessageResponseDto>();

                    return Results.Ok(response);
                })
            .RequireAuthorization()
            .WithTags("Conversations")
            .WithName("SendMessage")
            .Produces<SendMessageResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Send a message to a conversation")
            .WithDescription("Send a message to a conversation by conversation id");
    }
}

public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(x => x.Content).NotNull().NotEmpty().MaximumLength(500);
    }
}

internal class SendMessageCommandHandler(
    IConversationsRepository conversationsRepository)
    : ICommandHandler<SendMessageCommand, SendMessageResult>
{
    public async Task<SendMessageResult> Handle(SendMessageCommand command, CancellationToken cancellationToken)
    {
        var conversation = await conversationsRepository.GetConversationByIdAsync(command.ConversationId);
        if (conversation is null)
            throw new ConversationNotFoundException(command.ConversationId);

        var message = Message.Create(conversation.Id, command.ProfileId, command.Content);

        await conversationsRepository.AddMessageAsync(message);

        var messageDto = message.Adapt<MessageDto>();

        return new SendMessageResult(messageDto);
    }
}