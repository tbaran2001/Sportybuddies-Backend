namespace Sportybuddies.API.Common.Behaviors;

public class DomainEventsBehavior<TRequest, TResponse>(ApplicationDbContext context, IPublisher publisher)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next(cancellationToken);

        await PublishDomainEventsAsync(cancellationToken);

        return response;
    }

    private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
    {
        var entities = context.ChangeTracker.Entries<Entity>()
            .Where(entry => entry.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity)
            .ToList();

        var domainEvents = entities.SelectMany(entity => entity.DomainEvents).ToList();

        entities.ForEach(entity => entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent, cancellationToken);
        }
    }
}