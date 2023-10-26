using MediatR;

namespace AspNetCore.EventSourcing.Core.Abstractions.DomainEvents
{
    public abstract record DomainEvent() : INotification
    {
        public Guid EventId { get; } = Guid.NewGuid();
    }
}
