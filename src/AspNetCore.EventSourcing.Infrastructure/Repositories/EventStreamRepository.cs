using AspNetCore.EventSourcing.Application.Abstractions.Repositories;
using AspNetCore.EventSourcing.Core.Abstractions.DomainEvents;
using AspNetCore.EventSourcing.Core.Abstractions.Entities;
using AspNetCore.EventSourcing.Infrastructure.Repositories.EventStore;
using System.Collections.Concurrent;
using System.Reflection;

namespace AspNetCore.EventSourcing.Infrastructure.Repositories
{
    public sealed class EventStreamRepository<TAggregate> : IEventStreamRepository<TAggregate>
      where TAggregate : EventSourcedAggregate
    {
        private readonly IEventStore _eventStore;
        private readonly IUnitOfWork _unitOfWork;
        private ConcurrentDictionary<Guid, TAggregate> _trackedAggregates = new ConcurrentDictionary<Guid, TAggregate>();
        private static ConstructorInfo _rehydrationFactory;

        static EventStreamRepository()
        {
            _rehydrationFactory = typeof(TAggregate)
                                    .GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                                        null, new Type[0], new ParameterModifier[0])!;
        }

        public EventStreamRepository(IEventStore eventStore, 
            IUnitOfWork unitOfWork)
        {
            _eventStore = eventStore;
            _unitOfWork = unitOfWork;
        }

        public async Task<TAggregate> GetByIdAsync(Guid id)
        {
            if (!_trackedAggregates.TryGetValue(id, out var aggregate))
            {
                aggregate = CreateEmptyAggregate();

                foreach (var @event in await _eventStore.ReadEventsAsync(id))
                {
                    aggregate.ApplyEvent(@event.DomainEvent);
                }
                _trackedAggregates.TryAdd(id, aggregate);
            }
            return aggregate;
        }

        public async Task<List<Event>> GetEventsAsync(Guid id)
        {
            var events = await _eventStore.ReadEventsAsync(id);
            return events.ToList();
        }

        public async Task SaveAsync(TAggregate aggregate)
        {
            if (!_trackedAggregates.ContainsKey(aggregate.Id))
            {
                _trackedAggregates.TryAdd(aggregate.Id, aggregate);
            }
            await ProcessDomainEventsAsync(aggregate);
        }

        private async Task ProcessDomainEventsAsync(TAggregate aggregate)
        {
            var processedDomainEvents = new List<DomainEvent>();
            var unprocessedDomainEvents = aggregate.DomainEvents.ToList();

            while (unprocessedDomainEvents.Any())
            {
                await _eventStore.AppendEventsAsync(aggregate.Id, aggregate.GetType().Name,unprocessedDomainEvents);
                await _unitOfWork.DispatchDomainEventsAsync(unprocessedDomainEvents);
                processedDomainEvents.AddRange(unprocessedDomainEvents);
                unprocessedDomainEvents = aggregate.DomainEvents
                                            .Where(e => !processedDomainEvents.Contains(e))
                                            .ToList();
            }

            aggregate.ClearDomainEvents();
        }

        private TAggregate CreateEmptyAggregate()
        {
            return (TAggregate)_rehydrationFactory.Invoke(new object[0]);
        }
    }
}
