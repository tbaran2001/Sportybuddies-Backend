namespace Sportybuddies.API.Common.Models;

public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected Entity(Guid id)
    {
        Id = id;
    }

    protected Entity()
    {
    }

    public Guid Id { get; protected set; }

    public virtual IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public List<IDomainEvent> PopDomainEvents()
    {
        var copy = _domainEvents.ToList();

        _domainEvents.Clear();

        return copy;
    }
}