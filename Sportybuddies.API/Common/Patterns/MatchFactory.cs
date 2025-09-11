using Sportybuddies.API.Common.Patterns;
using Sportybuddies.API.Modules.Matches.Models;

namespace Sportybuddies.API.Common.Patterns;

public interface IMatchFactory
{
    Result<(Match, Match)> CreateMatchPair(Guid profileId, Guid matchedProfileId, DateTimeOffset matchDateTime);
}

public class MatchFactory : IMatchFactory
{
    public Result<(Match, Match)> CreateMatchPair(Guid profileId, Guid matchedProfileId, DateTimeOffset matchDateTime)
    {
        // Validation
        if (profileId == Guid.Empty)
            return Result.Failure<(Match, Match)>("ProfileId cannot be empty");
            
        if (matchedProfileId == Guid.Empty)
            return Result.Failure<(Match, Match)>("MatchedProfileId cannot be empty");
            
        if (profileId == matchedProfileId)
            return Result.Failure<(Match, Match)>("Cannot create match with the same profile");

        if (matchDateTime > DateTimeOffset.UtcNow)
            return Result.Failure<(Match, Match)>("Match date cannot be in the future");

        try
        {
            var matchPair = Match.CreatePair(profileId, matchedProfileId, matchDateTime);
            return Result.Success(matchPair);
        }
        catch (Exception ex)
        {
            return Result.Failure<(Match, Match)>($"Failed to create match pair: {ex.Message}");
        }
    }
}