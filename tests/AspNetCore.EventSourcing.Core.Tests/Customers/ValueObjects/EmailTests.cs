using AspNetCore.EventSourcing.Core.Abstractions.Exceptions;
using AspNetCore.EventSourcing.Core.Customers.ValueObjects;

namespace AspNetCore.EventSourcing.Core.Tests.Customers.ValueObjects
{
    public class EmailTests
    {
        [Fact]
        public void GivenEmail_WhenValid_ThenTrimAndCreate()
        {
            var email = Email.Create("Tester@test.com ");
            email.Value.Should().Be("tester@test.com");
        }

        [Fact]
        public void GivenEmail_WhenInvalid_ThenThrow()
        {
            Action action = () => Email.Create("tester");
            action.Should().Throw<DomainException>().WithMessage("Invalid Email");
        }
    }
}
