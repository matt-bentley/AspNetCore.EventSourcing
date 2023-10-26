using AspNetCore.EventSourcing.Core.Abstractions.DomainEvents;

namespace AspNetCore.EventSourcing.Core.Accounts.DomainEvents
{
    public sealed record AmountDepositedDomainEvent(Guid AccountId, decimal Amount, string? Description, decimal PreviousBalance, decimal NewBalance, DateTime Date) : DomainEvent;
}
