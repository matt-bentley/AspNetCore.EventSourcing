using AspNetCore.EventSourcing.Core.Abstractions.Exceptions;
using AspNetCore.EventSourcing.Core.Customers.ValueObjects;

namespace AspNetCore.EventSourcing.Core.Tests.Customers.ValueObjects
{
    public class NameTests
    {
        [Fact]
        public void GivenName_WhenCreateValid_ThenTrimAndCreate()
        {
            var name = Name.Create("Joe ", " Bloggs");
            name.FirstName.Should().Be("Joe");
            name.LastName.Should().Be("Bloggs");
        }

        [Fact]
        public void GivenName_WhenNullFirstName_ThenError()
        {
            Action action = () => Name.Create(null, "test");
            action.Should().Throw<DomainException>().WithMessage("Required input 'FirstName' is missing.");
        }

        [Fact]
        public void GivenName_WhenBlankLastName_ThenError()
        {
            Action action = () => Name.Create("Tester", " ");
            action.Should().Throw<DomainException>().WithMessage("Required input 'LastName' is missing.");
        }
    }
}
