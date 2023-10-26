using AspNetCore.EventSourcing.Core.Abstractions.DomainEvents;

namespace AspNetCore.EventSourcing.Core.Accounts.DomainEvents
{
    public sealed record AccountClosedDomainEvent(Guid Id, DateTime ClosedDate) : DomainEvent;
}
