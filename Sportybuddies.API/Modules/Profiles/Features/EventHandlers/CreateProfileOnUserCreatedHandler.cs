namespace Sportybuddies.API.Modules.Profiles.Features.EventHandlers;

public class CreateProfileOnUserCreatedHandler(ISender sender) : INotificationHandler<UserCreatedEvent>
{
    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var command = new CreateProfileCommand(
            Guid.NewGuid(),
            notification.Email,
            DateTimeOffset.Now - TimeSpan.FromDays(365 * 20),
            Gender.Male,
            notification.UserId);
        await sender.Send(command, cancellationToken);
    }
}