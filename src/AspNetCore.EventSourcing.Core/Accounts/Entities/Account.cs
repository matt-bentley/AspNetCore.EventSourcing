using AspNetCore.EventSourcing.Core.Abstractions.Entities;
using AspNetCore.EventSourcing.Core.Abstractions.Exceptions;
using AspNetCore.EventSourcing.Core.Abstractions.Guards;
using AspNetCore.EventSourcing.Core.Accounts.DomainEvents;

namespace AspNetCore.EventSourcing.Core.Accounts.Entities
{
    public sealed class Account : EventSourcedAggregate
    {
        private Account(Guid id, string accountNumber, Guid customerId)
        {
            AddEvent(new AccountOpenedDomainEvent(id, accountNumber, customerId, DateTime.UtcNow));
        }

        private Account()
        {
            // needed for re-hydration
        }

        public static Account Create(Guid id, string? accountNumber, Guid customerId)
        {
            accountNumber = (accountNumber ?? string.Empty).Trim();
            Guard.Against.NullOrEmpty(accountNumber, nameof(AccountNumber));
            return new Account(id, accountNumber, customerId);
        }

        public string AccountNumber { get; private set; }
        public Guid CustomerId { get; private set; }
        public bool Closed { get; private set; }
        public decimal Balance { get; private set; }

        public void Withdraw(decimal amount, string? description)
        {
            CheckAccountIsOpen();
            Guard.Against.LessThanOrEqualTo(amount, 0, nameof(amount));
            var newBalance = Balance - amount;
            Guard.Against.LessThan(newBalance, 0, message: "Insufficient funds for withdraw");
            AddEvent(new AmountWithdrawnDomainEvent(Id, amount, description, Balance, newBalance, DateTime.UtcNow));
        }

        public void Deposit(decimal amount, string? description)
        {
            CheckAccountIsOpen();
            Guard.Against.LessThanOrEqualTo(amount, 0, nameof(amount));
            var newBalance = Balance + amount;
            AddEvent(new AmountDepositedDomainEvent(Id, amount, description, Balance, newBalance, DateTime.UtcNow));
        }

        public void Close()
        {
            CheckAccountIsOpen();
            AddEvent(new AccountClosedDomainEvent(Id, DateTime.UtcNow));
        }

        private void CheckAccountIsOpen()
        {
            if (Closed)
            {
                throw new DomainException("Account is closed");
            }
        }

        internal void Apply(AccountOpenedDomainEvent @event)
        {
            Id = @event.Id;
            CustomerId = @event.CustomerId;
            AccountNumber = @event.AccountNumber;
            Balance = 0;
        }

        internal void Apply(AmountWithdrawnDomainEvent @event)
        {
            Balance = @event.NewBalance;
        }

        internal void Apply(AmountDepositedDomainEvent @event)
        {
            Balance = @event.NewBalance;
        }

        internal void Apply(AccountClosedDomainEvent @event)
        {
            Closed = true;
        }
    }
}
