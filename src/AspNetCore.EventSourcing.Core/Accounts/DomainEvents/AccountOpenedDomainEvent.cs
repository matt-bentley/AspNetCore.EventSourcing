using AspNetCore.EventSourcing.Core.Abstractions.DomainEvents;

namespace AspNetCore.EventSourcing.Core.Accounts.DomainEvents
{
    public sealed record AccountOpenedDomainEvent(Guid Id, string AccountNumber, Guid CustomerId, DateTime OpenedDate) : DomainEvent;
}
