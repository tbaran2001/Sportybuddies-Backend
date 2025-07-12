namespace Sportybuddies.API.Data.Repositories;

public class ConversationsRepository(ApplicationDbContext dbContext) : IConversationsRepository
{
    public async Task AddConversationAsync(Conversation conversation)
    {
        await dbContext.Conversations.AddAsync(conversation);
    }

    public async Task<Conversation> GetConversationByIdAsync(Guid id)
    {
        return await dbContext.Conversations
            .Include(c => c.Participants)
            .ThenInclude(p => p.Profile)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddMessageAsync(Message message)
    {
        await dbContext.Messages.AddAsync(message);
    }

    public async Task<Conversation> GetConversationByIdWithMessagesAsync(Guid conversationId)
    {
        return await dbContext.Conversations
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Id == conversationId);
    }

    public async Task<IEnumerable<Message>> GetLastMessageFromEachProfileConversationAsync(Guid profileId)
    {
        return await dbContext.Conversations
            .Where(c => c.Participants.Any(p => p.ProfileId == profileId))
            .SelectMany(c => c.Messages)
            .GroupBy(m => m.ConversationId)
            .Select(g => g.OrderByDescending(m => m.CreatedOn).FirstOrDefault())
            .ToListAsync();
    }

    public async Task<bool> ProfilesHaveConversationAsync(Guid firstUserId, Guid secondUserId)
    {
        return await dbContext.Conversations
            .Where(c => c.Participants.Any(p => p.ProfileId == firstUserId))
            .Where(c => c.Participants.Any(p => p.ProfileId == secondUserId))
            .AnyAsync();
    }
}