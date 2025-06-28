namespace Sportybuddies.API.Modules.Profiles.ValueObjects;

public record Location
{
    public double Latitude { get; }
    public double Longitude { get; }
    public string Address { get; }

    private Location(double latitude, double longitude, string address)
    {
        Latitude = latitude;
        Longitude = longitude;
        Address = address;
    }

    public static Location Create(double latitude, double longitude, string address)
    {
        if (latitude < -90 || latitude > 90)
            throw new ArgumentException("Latitude must be in range from -90 to 90");

        if (longitude < -180 || longitude > 180)
            throw new ArgumentException("Longitude must be in range from -180 to 180");

        return new Location(latitude, longitude, address);
    }

    public double CalculateDistance(Location other)
    {
        const double earthRadiusKm = 6371;
        var latDistance = ToRadians(other.Latitude - Latitude);
        var lonDistance = ToRadians(other.Longitude - Longitude);

        var a = Math.Sin(latDistance / 2) * Math.Sin(latDistance / 2) +
                Math.Cos(ToRadians(Latitude)) * Math.Cos(ToRadians(other.Latitude)) *
                Math.Sin(lonDistance / 2) * Math.Sin(lonDistance / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return earthRadiusKm * c;
    }

    private static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    private Location()
    {
    }
}