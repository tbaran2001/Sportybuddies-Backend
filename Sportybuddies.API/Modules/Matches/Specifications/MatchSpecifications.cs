using Sportybuddies.API.Common.Patterns;
using Sportybuddies.API.Modules.Matches.Models;

namespace Sportybuddies.API.Modules.Matches.Specifications;

public class MatchByIdSpecification : BaseSpecification<Match>
{
    public MatchByIdSpecification(Guid matchId) : base(m => m.Id == matchId)
    {
    }
}

public class MatchesByProfileIdSpecification : BaseSpecification<Match>
{
    public MatchesByProfileIdSpecification(Guid profileId) : base(m => m.ProfileId == profileId)
    {
    }
}

public class ExistingMatchesForProfileSpecification : BaseSpecification<Match>
{
    public ExistingMatchesForProfileSpecification(Guid profileId) 
        : base(m => m.ProfileId == profileId || m.MatchedProfileId == profileId)
    {
    }
}

public class ActiveMatchesForProfileSpecification : BaseSpecification<Match>
{
    public ActiveMatchesForProfileSpecification(Guid profileId) 
        : base(m => m.ProfileId == profileId && m.Swipe == null)
    {
        AddInclude(m => m.Profile);
        AddInclude(m => m.MatchedProfile);
        AddInclude("MatchedProfile.Sports");
    }
}

public class RandomMatchForProfileSpecification : BaseSpecification<Match>
{
    public RandomMatchForProfileSpecification(Guid profileId) 
        : base(m => m.ProfileId == profileId && m.Swipe == null)
    {
        AddInclude(m => m.Profile);
        AddInclude(m => m.MatchedProfile);
        AddInclude("MatchedProfile.Sports");
    }
}