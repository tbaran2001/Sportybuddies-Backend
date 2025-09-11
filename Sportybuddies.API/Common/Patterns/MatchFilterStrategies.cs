using Sportybuddies.API.Modules.Matches.Dtos;
using Sportybuddies.API.Modules.Profiles.Models;

namespace Sportybuddies.API.Common.Patterns;

public interface IMatchFilterStrategy
{
    bool IsMatch(Profile profile, Profile matchedProfile, MatchDto match);
}

public class AgeFilterStrategy : IMatchFilterStrategy
{
    public bool IsMatch(Profile profile, Profile matchedProfile, MatchDto match)
    {
        if (profile.Preferences == null) return false;
        
        var age = CalculateAge(matchedProfile.DateOfBirth);
        return age >= profile.Preferences.MinAge && age <= profile.Preferences.MaxAge;
    }
    
    private static int CalculateAge(DateTimeOffset dateOfBirth)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - dateOfBirth.Year;
        if (DateOnly.FromDateTime(dateOfBirth.Date) > today.AddYears(-age))
            age--;
        return age;
    }
}

public class GenderFilterStrategy : IMatchFilterStrategy
{
    public bool IsMatch(Profile profile, Profile matchedProfile, MatchDto match)
    {
        if (profile.Preferences == null) return false;
        
        return profile.Preferences.PreferredGender == Gender.Unknown || 
               matchedProfile.Gender == profile.Preferences.PreferredGender;
    }
}

public class DistanceFilterStrategy : IMatchFilterStrategy
{
    public bool IsMatch(Profile profile, Profile matchedProfile, MatchDto match)
    {
        if (profile.Preferences == null || profile.Location == null || matchedProfile.Location == null) 
            return false;
        
        var distance = profile.Location.CalculateDistance(matchedProfile.Location);
        return distance <= profile.Preferences.MaxDistance;
    }
}

public class CompositeMatchFilterStrategy : IMatchFilterStrategy
{
    private readonly List<IMatchFilterStrategy> _strategies;
    
    public CompositeMatchFilterStrategy(params IMatchFilterStrategy[] strategies)
    {
        _strategies = strategies.ToList();
    }
    
    public bool IsMatch(Profile profile, Profile matchedProfile, MatchDto match)
    {
        return _strategies.All(strategy => strategy.IsMatch(profile, matchedProfile, match));
    }
}