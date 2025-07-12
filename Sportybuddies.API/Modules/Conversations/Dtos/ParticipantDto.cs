namespace Sportybuddies.API.Modules.Conversations.Dtos;

public record ParticipantDto(Guid Id, Guid ConversationId, ProfileDto Profile, DateTimeOffset CreatedOn);