
namespace AspNetCore.EventSourcing.Core.Abstractions.Entities
{
    public sealed class EventStream : AggregateRoot
    {
        private EventStream(Guid id, string aggregateType, int version, DateTime createdDate) : base(id)
        {
            AggregateType = aggregateType;
            Version = version;
            CreatedDate = createdDate;
        }

        public static EventStream Create(Guid id,string aggregateType, int version = 0)
        {
            return new EventStream(id, aggregateType, version, DateTime.UtcNow);
        }

        public DateTime CreatedDate { get; private set; }
        public string AggregateType { get; private set; }
        public int Version { get; private set; }
        public List<Event> Events { get; private set; } = new List<Event>();

        public void AddEvent(Event @event)
        {
            Version++;
            Events.Add(@event);
        }
    }
}
