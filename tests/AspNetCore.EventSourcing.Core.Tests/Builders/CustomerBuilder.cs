using AspNetCore.EventSourcing.Core.Customers.Entities;
using AspNetCore.EventSourcing.Core.Customers.ValueObjects;

namespace AspNetCore.EventSourcing.Core.Tests.Builders
{
    public class CustomerBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _firstName = "Joe";
        private string _lastName = "Bloggs";
        private string _email = "joebloggs@test.com";

        public Customer Build()
        {
            var name = Name.Create(_firstName, _lastName);
            var email = Email.Create(_email);
            return Customer.Create(_id, name, email);
        }

        public CustomerBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }
    }
}
