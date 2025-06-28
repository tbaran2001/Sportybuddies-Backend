namespace Sportybuddies.API.Modules.Sports.Models;

public class Sport : Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    public static Sport Create(string name, string description)
    {
        var sport = new Sport
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description
        };

        return sport;
    }
}