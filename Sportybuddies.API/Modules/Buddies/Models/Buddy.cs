namespace Sportybuddies.API.Modules.Buddies.Models;

public class Buddy : Entity
{
    public Guid OppositeBuddyId { get; private set; }
    public Guid ProfileId { get; private set; }
    public Guid MatchedProfileId { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public Guid? ConversationId { get; private set; }

    public Profile Profile { get; private set; }
    public Profile MatchedProfile { private set; get; }
    public Conversation Conversation { get; private set; }

    public static (Buddy, Buddy) CreatePair(Guid profileId, Guid matchedProfileId, DateTime createdOn)
    {
        var buddy1 = new Buddy
        {
            Id = Guid.NewGuid(),
            ProfileId = profileId,
            MatchedProfileId = matchedProfileId,
            CreatedOn = createdOn,
        };
        var buddy2 = new Buddy
        {
            Id = Guid.NewGuid(),
            ProfileId = matchedProfileId,
            MatchedProfileId = profileId,
            CreatedOn = createdOn,
        };

        buddy1.OppositeBuddyId = buddy2.Id;
        buddy2.OppositeBuddyId = buddy1.Id;

        return (buddy1, buddy2);
    }

    public void SetConversation(Conversation conversation)
    {
        Conversation = conversation;
        ConversationId = conversation.Id;
    }
}