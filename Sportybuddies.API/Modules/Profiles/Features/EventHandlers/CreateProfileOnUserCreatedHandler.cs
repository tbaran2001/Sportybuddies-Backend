using Bogus;

namespace Sportybuddies.API.Modules.Profiles.Features.EventHandlers;

public class CreateProfileOnUserCreatedHandler(
    ISender sender,
    ISportsRepository sportsRepository,
    ApplicationDbContext dbContext) : INotificationHandler<UserCreatedEvent>
{
    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var faker = new Faker();
        var command = new CreateProfileCommand(
            Guid.NewGuid(),
            faker.Person.UserName,
            new DateTimeOffset(faker.Date.Past(70, new DateTime(2006, 1, 1))),
            new Random().Next(0, 2) == 0 ? Gender.Male : Gender.Female,
            notification.UserId);
        await sender.Send(command, cancellationToken);
    }
}