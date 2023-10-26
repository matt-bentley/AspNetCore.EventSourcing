using AspNetCore.EventSourcing.Core.Abstractions.DomainEvents;

namespace AspNetCore.EventSourcing.Core.Accounts.DomainEvents
{
    public sealed record AmountWithdrawnDomainEvent(Guid AccountId, decimal Amount, string? Description, decimal PreviousBalance, decimal NewBalance, DateTime Date) : DomainEvent;
}
