using AspNetCore.EventSourcing.Core.Abstractions.DomainEvents;
using AspNetCore.EventSourcing.Core.Abstractions.Entities;

namespace AspNetCore.EventSourcing.Infrastructure.Repositories.EventStore
{
    public interface IEventStore
    {
        Task<IEnumerable<Event>> ReadEventsAsync(Guid id);
        Task<AppendResult> AppendEventsAsync(Guid id, string aggregateType, List<DomainEvent> events);
    }
}
