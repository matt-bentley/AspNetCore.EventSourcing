using AspNetCore.EventSourcing.Core.Abstractions.DomainEvents;

namespace AspNetCore.EventSourcing.Core.Abstractions.Entities
{
    public abstract class EventSourcedAggregate : AggregateRoot
    {
        public void ApplyEvent(DomainEvent @event)
        {
            // this is needed to dynamically find the correct methods
            // on the parent class
            ((dynamic)this).Apply((dynamic)@event);
        }

        protected void AddEvent(DomainEvent @event)
        {
            ApplyEvent(@event);
            base.AddDomainEvent(@event);
        }

        protected override void AddDomainEvent(DomainEvent eventItem)
        {
            throw new NotSupportedException($"Please use {nameof(AddEvent)} instead.");
        }
    }
}
