namespace Sportybuddies.API.Data.Repositories;

public interface IConversationsRepository
{
    Task AddConversationAsync(Conversation conversation);
    Task<Conversation> GetConversationByIdAsync(Guid id);
    Task AddMessageAsync(Message message);
    Task<Conversation> GetConversationByIdWithMessagesAsync(Guid conversationId);
    Task<IEnumerable<Message>> GetLastMessageFromEachProfileConversationAsync(Guid profileId);
    Task<bool> ProfilesHaveConversationAsync(Guid firstProfileId, Guid secondProfileId);
    Task<IEnumerable<Conversation>> GetConversationsByProfileIdAsync(Guid profileId);
    Task<Conversation> GetLatestConversationByProfileIdAsync(Guid profileId);
}