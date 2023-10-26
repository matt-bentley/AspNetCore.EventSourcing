using AspNetCore.EventSourcing.Application.Abstractions.Repositories;
using AspNetCore.EventSourcing.Core.Abstractions.DomainEvents;
using AspNetCore.EventSourcing.Core.Abstractions.Entities;
using AspNetCore.EventSourcing.Core.Abstractions.Exceptions;

namespace AspNetCore.EventSourcing.Infrastructure.Repositories.EventStore
{
    public sealed class EntityFrameworkEventStore : IEventStore
    {
        private readonly IRepository<EventStream> _repository;

        public EntityFrameworkEventStore(IRepository<EventStream> repository)
        {
            _repository = repository;
        }

        public async Task<AppendResult> AppendEventsAsync(Guid id, string aggregateType, List<DomainEvent> events)
        {
            var eventStream = await GetEventStreamAsync(id);

            if (eventStream == null)
            {
                eventStream = EventStream.Create(id, aggregateType);
                _repository.Insert(eventStream);
            }

            foreach (var @event in events)
            {
                var streamEvent = new Event(@event, eventStream.Version + 1, DateTime.UtcNow);
                eventStream.AddEvent(streamEvent);
            }
            
            var result = new AppendResult(eventStream.Version);
            return result;
        }

        public async Task<IEnumerable<Event>> ReadEventsAsync(Guid id)
        {
            var eventStream = await FindEventStreamAsync(id);
            return eventStream.Events;
        }

        private async Task<EventStream> FindEventStreamAsync(Guid id)
        {
            var eventStream = await GetEventStreamAsync(id);
            if (eventStream == null)
            {
                throw new NotFoundException();
            }
            return eventStream;
        }

        private async Task<EventStream?> GetEventStreamAsync(Guid id)
        {
            var eventStream = await _repository.GetByIdAsync(id);
            return eventStream;
        }
    }
}
