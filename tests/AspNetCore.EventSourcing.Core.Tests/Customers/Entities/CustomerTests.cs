using AspNetCore.EventSourcing.Core.Customers.ValueObjects;
using AspNetCore.EventSourcing.Core.Tests.Builders;

namespace AspNetCore.EventSourcing.Core.Tests.Customers.Entities
{
    public class CustomerTests
    {
        [Fact]
        public void GivenCustomer_WhenCreateValid_ThenCreate()
        {
            var customer = new CustomerBuilder().Build();
            customer.Should().NotBeNull();
            customer.JoinedDate.Should().BeBefore(DateTime.UtcNow);
            customer.HasLeft.Should().BeFalse();
            customer.LeftDate.Should().BeNull();
        }

        [Fact]
        public void GivenCustomer_WhenUpdateName_ThenUpdate()
        {
            var customer = new CustomerBuilder().Build();
            var name = Name.Create("Updated", "Name");
            customer.UpdateName(name);
            customer.Name.FirstName.Should().Be("Updated");
        }

        [Fact]
        public void GivenCustomer_WhenLeave_ThenUpdateHasLeft()
        {
            var customer = new CustomerBuilder().Build();
            customer.Leave();
            customer.HasLeft.Should().BeTrue();
            customer.LeftDate.Should().BeBefore(DateTime.UtcNow);
        }
    }
}
