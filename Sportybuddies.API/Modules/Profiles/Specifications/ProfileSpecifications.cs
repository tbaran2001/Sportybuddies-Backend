using Sportybuddies.API.Common.Patterns;
using Sportybuddies.API.Modules.Profiles.Models;

namespace Sportybuddies.API.Modules.Profiles.Specifications;

public class ProfileByIdSpecification : BaseSpecification<Profile>
{
    public ProfileByIdSpecification(Guid profileId) : base(p => p.Id == profileId)
    {
    }
}

public class ProfileByIdWithSportsSpecification : BaseSpecification<Profile>
{
    public ProfileByIdWithSportsSpecification(Guid profileId) : base(p => p.Id == profileId)
    {
        AddInclude(p => p.Sports);
    }
}

public class ProfileByUserIdWithSportsSpecification : BaseSpecification<Profile>
{
    public ProfileByUserIdWithSportsSpecification(Guid userId) : base(p => p.UserId == userId)
    {
        AddInclude(p => p.Sports);
    }
}

public class AllProfilesWithSportsSpecification : BaseSpecification<Profile>
{
    public AllProfilesWithSportsSpecification()
    {
        AddInclude(p => p.Sports);
    }
}

public class PotentialMatchesSpecification : BaseSpecification<Profile>
{
    public PotentialMatchesSpecification(Guid profileId, IEnumerable<Guid> profileSports) 
        : base(p => p.Id != profileId && p.Sports.Any(s => profileSports.Contains(s.Id)))
    {
        // Note: The complex query logic for excluding existing matches would need to be handled
        // in the repository or through a more sophisticated specification pattern
    }
}