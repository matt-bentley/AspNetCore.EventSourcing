using AspNetCore.EventSourcing.Core.Abstractions.DomainEvents;

namespace AspNetCore.EventSourcing.Core.Abstractions.Entities
{
    public abstract class AggregateRoot : EntityBase
    {
        protected AggregateRoot() : this(Guid.NewGuid())
        {

        }

        protected AggregateRoot(Guid id)
        {
            Id = id;
        }

        private readonly List<DomainEvent> _domainEvents = new List<DomainEvent>();
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected virtual void AddDomainEvent(DomainEvent eventItem)
        {
            _domainEvents.Add(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
