using AspNetCore.EventSourcing.Core.Abstractions.Exceptions;
using CSharpFunctionalExtensions;
using System.Net.Mail;

namespace AspNetCore.EventSourcing.Core.Customers.ValueObjects
{
    public sealed class Email : ValueObject
    {
        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string? email)
        {
            email = (email ?? string.Empty).ToLower().Trim();
            try
            {
                _ = new MailAddress(email);
            }
            catch
            {
                throw new DomainException("Invalid Email");
            }
            return new Email(email);
        }

        public string Value { get; private set; }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
