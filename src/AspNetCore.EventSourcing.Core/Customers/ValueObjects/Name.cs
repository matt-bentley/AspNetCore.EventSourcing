using AspNetCore.EventSourcing.Core.Abstractions.Guards;
using CSharpFunctionalExtensions;

namespace AspNetCore.EventSourcing.Core.Customers.ValueObjects
{
    public sealed class Name : ValueObject
    {
        private Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public static Name Create(string? firstName, string? lastName)
        {
            firstName = (firstName ?? string.Empty).Trim();
            Guard.Against.NullOrEmpty(firstName, nameof(FirstName));

            lastName = (lastName ?? string.Empty).Trim();
            Guard.Against.NullOrEmpty(lastName, nameof(LastName));

            return new Name(firstName, lastName);
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
        }
    }
}
