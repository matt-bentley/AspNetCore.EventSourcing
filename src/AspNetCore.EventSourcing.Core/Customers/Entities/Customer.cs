using AspNetCore.EventSourcing.Core.Abstractions.Entities;
using AspNetCore.EventSourcing.Core.Customers.ValueObjects;

namespace AspNetCore.EventSourcing.Core.Customers.Entities
{
    public sealed class Customer : AggregateRoot
    {
        private Customer(Guid id, Name name, Email email, DateTime joinedDate, bool hasLeft, DateTime? leftDate) : base(id)
        {
            Name = name;
            Email = email;
            JoinedDate = joinedDate;
            HasLeft = hasLeft;
            LeftDate = leftDate;
        }

        private Customer()
        {
            // needed for EF
        }

        public static Customer Create(Guid id, Name name, Email email)
        {
            return new Customer(id, name, email, DateTime.UtcNow, false, null);
        }

        public Name Name { get; private set; }
        public Email Email { get; private set; }
        public DateTime JoinedDate { get; private set; }
        public bool HasLeft { get; private set; }
        public DateTime? LeftDate { get; private set; }

        public void UpdateName(Name name)
        {
            Name = name;
        }

        public void Leave()
        {
            if (!HasLeft)
            {
                HasLeft = true;
                LeftDate = DateTime.UtcNow;
            }
        }
    }
}
