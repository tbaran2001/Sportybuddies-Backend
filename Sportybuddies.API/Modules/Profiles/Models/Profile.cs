namespace Sportybuddies.API.Modules.Profiles.Models;

public class Profile : Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string MainPhotoUrl { get; private set; }
    public DateTimeOffset CreatedOn { get; private set; }
    public DateTimeOffset DateOfBirth { get; private set; }
    public Gender Gender { get; private set; }
    public Preferences Preferences { get; private set; }
    public Location Location { get; private set; }
    public ICollection<Sport> Sports { get; private set; } = new List<Sport>();
    public Guid UserId { get; set; }
    public virtual ApplicationUser User { get; set; }

    public static Profile Create(Guid id, string name, DateTimeOffset dateOfBirth, Gender gender, Guid userId)
    {
        var profile = new Profile
        {
            Id = id,
            Name = name,
            DateOfBirth = dateOfBirth,
            Gender = gender,
            UserId = userId,
            CreatedOn = DateTimeOffset.UtcNow,
            Preferences = Preferences.Default,
            Location = null,
            Description = null,
            MainPhotoUrl = null,
        };

        return profile;
    }

    public static Profile Create(Guid id)
    {
        var profile = new Profile
        {
            Id = id,
        };

        return profile;
    }

    public void Update(string name, string description, DateTimeOffset dateOfBirth, Gender gender)
    {
        Name = name;
        Description = description;
        DateOfBirth = dateOfBirth;
        Gender = gender;
    }

    public void AddSport(Sport sport)
    {
        if (Sports.Contains(sport))
            throw new Exception("Profile already has this sport");

        Sports.Add(sport);
    }

    public void RemoveSport(Sport sport)
    {
        if (!Sports.Contains(sport))
            throw new Exception("Profile does not have this sport");

        Sports.Remove(sport);
    }

    public void UpdatePreferences(Preferences preferences)
    {
        Preferences = preferences;
    }

    public void UpdateLocation(Location location)
    {
        Location = location;
    }

    public void AddMainPhoto(string url)
    {
        MainPhotoUrl = url;
    }

    public void RemoveMainPhoto()
    {
        MainPhotoUrl = null;
    }

    public void SetMainPhotoUrl(string url)
    {
        MainPhotoUrl = url;
    }
}