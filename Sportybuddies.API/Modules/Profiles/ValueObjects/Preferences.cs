namespace Sportybuddies.API.Modules.Profiles.ValueObjects;

public record Preferences
{
    public int MinAge { get; }
    public int MaxAge { get; }
    public int MaxDistance { get; }
    public Gender PreferredGender { get; }

    public static Preferences Default => new Preferences(18, 45, 50, Gender.Unknown);

    private Preferences(int minAge, int maxAge, int maxDistance, Gender preferredGender)
    {
        MinAge = minAge;
        MaxAge = maxAge;
        MaxDistance = maxDistance;
        PreferredGender = preferredGender;
    }

    public static Preferences Create(int minAge, int maxAge, int maxDistance, Gender gender)
    {
        if (minAge < 0 || maxAge < 0)
            throw new ArgumentException("Age cannot be negative");

        if (minAge > maxAge)
            throw new ArgumentException("Min age cannot be greater than max age");

        if (maxDistance is < 1 or > 100)
            throw new ArgumentException("Max distance must be in range from 1 to 100");

        return new Preferences(minAge, maxAge, maxDistance, gender);
    }

    private Preferences()
    {
    }
}