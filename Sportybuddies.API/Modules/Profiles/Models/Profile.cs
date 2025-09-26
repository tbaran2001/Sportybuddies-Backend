using Bogus;

namespace Sportybuddies.API.Modules.Profiles.Models;

class City
{
    public double latitude { get; set; }
    public double longitude { get; set; }
    public string address { get; set; }
}

static class PolishCitiesGenerator
{
    public static List<City> PolishCities =
    [
        new()
        {
            latitude = 51.107883,
            longitude = 17.038538,
            address = "Wroclaw"
        },
        new()
        {
            address = "Krakow",
            latitude = 50.064739,
            longitude = 19.945020,
        },
        new()
        {
            address = "Warsaw",
            latitude = 52.229676,
            longitude = 21.012229,
        },
        new()
        {
            address = "Poznan",
            latitude = 52.406374,
            longitude = 16.925168,
        },
        new()
        {
            address = "Katowice",
            latitude = 50.263159,
            longitude = 19.015513,
        },
        new()
        {
            address = "Gdansk",
            latitude = 54.351007,
            longitude = 18.645198,
        }
    ];

    public static Location RandomLocation()
    {
        var randomCity = PolishCities[new Random().Next(0, PolishCities.Count)];
        return Location.Create(randomCity.latitude, randomCity.longitude, randomCity.address);
    }
}

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
            Location = PolishCitiesGenerator.RandomLocation(),
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

    public void UpdatePartial(string name, string description, DateTimeOffset? dateOfBirth, Gender? gender)
    {
        if (!string.IsNullOrEmpty(name))
            Name = name;

        if (!string.IsNullOrEmpty(description))
            Description = description;

        if (dateOfBirth.HasValue)
            DateOfBirth = dateOfBirth.Value;

        if (gender.HasValue)
            Gender = gender.Value;
    }

    public void AddSport(Sport sport)
    {
        if (Sports.Any(s => s.Id == sport.Id))
            throw new ProfileAlreadyHasSportException(Id, sport.Id);

        Sports.Add(sport);

        AddDomainEvent(new ProfileSportAddedDomainEvent(Id, sport.Id));
    }

    public void RemoveSport(Sport sport)
    {
        var sportToRemove = Sports.FirstOrDefault(s => s.Id == sport.Id);
        if (sportToRemove == null)
            throw new ProfileDoesNotHaveSportException(Id, sport.Id);

        Sports.Remove(sport);

        AddDomainEvent(new ProfileSportRemovedDomainEvent(Id, sport.Id));
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