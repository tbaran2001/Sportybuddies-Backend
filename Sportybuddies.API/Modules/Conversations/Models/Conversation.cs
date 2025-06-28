namespace Sportybuddies.API.Modules.Conversations.Models;

public class Conversation : Entity
{
    public Guid CreatorId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public ICollection<Participant> Participants { get; private set; }
    public ICollection<Message> Messages { get; private set; } = new List<Message>();

    public Profile Creator { get; private set; }

    public static Conversation CreateOneToOne(Guid creatorId, Guid participantId)
    {
        if (creatorId == participantId)
            throw new InvalidOperationException("A creator cannot have a one-to-one conversation with themselves.");

        var id = Guid.NewGuid();
        var conversation = new Conversation
        {
            Id = id,
            CreatorId = creatorId,
            CreatedOnUtc = DateTime.UtcNow,
            Participants = new List<Participant>
            {
                Participant.Create(id, creatorId),
                Participant.Create(id, participantId)
            }
        };

        return conversation;
    }
}