namespace Sportybuddies.API.Modules.Profiles.Features.EventHandlers;

public class CreateUserProfileHandler(ApplicationDbContext dbContext) : INotificationHandler<UserCreatedEvent>
{
    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var profileExists = await dbContext.Profiles.AnyAsync(p => p.UserId == notification.UserId, cancellationToken);
        if (profileExists)
        {
            throw new ProfileAlreadyExistsException(notification.UserId);
        }

        var newProfile = Profile.Create(Guid.NewGuid(), notification.Email, DateOnly.MaxValue, Gender.Female,
            notification.UserId);

        dbContext.Profiles.Add(newProfile);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}