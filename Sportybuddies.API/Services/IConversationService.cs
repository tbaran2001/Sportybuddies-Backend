namespace Sportybuddies.API.Services;

public interface IConversationService
{
    Task<Conversation> CreateConversationAsync(Guid creatorId, Guid participantId);
}