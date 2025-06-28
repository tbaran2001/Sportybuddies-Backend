namespace Sportybuddies.API.Modules.Conversations.Models;

public class Message : Entity
{
    public Guid ConversationId { get; private set; }
    public Guid SenderId { get; private set; }
    public string Content { get; private set; }
    public DateTimeOffset CreatedOn { get; private set; }

    public Conversation Conversation { get; private set; }
    public Profile Sender { get; private set; }

    public static Message Create(Guid conversationId, Guid senderId, string content)
    {
        var message = new Message
        {
            Id = Guid.NewGuid(),
            ConversationId = conversationId,
            SenderId = senderId,
            Content = content,
            CreatedOn = DateTimeOffset.UtcNow
        };

        return message;
    }
}