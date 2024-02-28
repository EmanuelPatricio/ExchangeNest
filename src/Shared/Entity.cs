namespace Shared;
public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected Entity(int id)
    {
        Id = id;
    }

    public int Id { get; init; }

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.ToList();
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}