using AspNetCore.EventSourcing.Core.Abstractions.Entities;

namespace AspNetCore.EventSourcing.Application.Abstractions.Repositories
{
    public interface IEventStreamRepository<TAggregate> where TAggregate : EventSourcedAggregate
    {
        Task<TAggregate?> GetByIdAsync(Guid id);
        Task<List<Event>> GetEventsAsync(Guid id);
        Task SaveAsync(TAggregate aggregate);
    }
}
