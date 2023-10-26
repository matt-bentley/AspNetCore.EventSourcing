using AspNetCore.EventSourcing.Application.Customers.Models;
using AspNetCore.EventSourcing.Core.Customers.Entities;
using AspNetCore.EventSourcing.Core.Customers.ValueObjects;

namespace AspNetCore.EventSourcing.Api.Tests.Controllers
{
    public class CustomersControllerTests
    {
        private const string BASE_URL = "api/customers";
        private readonly TestWebApplication _application = new TestWebApplication();

        [Fact]
        public async Task GivenCustomersController_WhenGet_ThenOk()
        {
            using var client = _application.CreateClient();
            var response = await client.GetAsync(BASE_URL);

            var customers = await response.ReadAndAssertSuccessAsync<List<CustomerDto>>();

            customers.Should().HaveCount(_application.TestCustomers.Count);
        }

        [Fact]
        public async Task GivenCustomersController_WhenGet_ThenExcludeDeleted()
        {
            foreach (var customer in _application.TestCustomers)
            {
                customer.Leave();
            }
            using var client = _application.CreateClient();
            var response = await client.GetAsync(BASE_URL);

            var customers = await response.ReadAndAssertSuccessAsync<List<CustomerDto>>();

            customers.Should().HaveCount(0);
        }

        [Fact]
        public async Task GivenCustomersController_WhenGetById_ThenOk()
        {
            using var client = _application.CreateClient();
            var response = await client.GetAsync($"{BASE_URL}/{_application.TestCustomers.First().Id}");

            var customer = await response.ReadAndAssertSuccessAsync<CustomerDto>();

            customer.Should().NotBeNull();
        }

        [Fact]
        public async Task GivenCustomersController_WhenPost_ThenCreated()
        {
            using var client = _application.CreateClient();

            var customer = new CustomerCreateDto()
            {
                Email = "new@test.com",
                FirstName = "New",
                LastName = "Tester"
            };

            var response = await client.PostAsync(BASE_URL, _application.GetStringContent(customer));

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            _application.CustomersRepository.Verify(e => e.Insert(It.IsAny<Customer>()), Times.Once);
            _application.UnitOfWork.Verify(e => e.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GivenCustomersController_WhenPostDuplicateEmail_ThenBadRequest()
        {
            using var client = _application.CreateClient();

            var customer = new CustomerCreateDto()
            {
                Email = _application.TestCustomers.First().Email.Value,
                FirstName = "New",
                LastName = "Tester"
            };

            var response = await client.PostAsync(BASE_URL, _application.GetStringContent(customer));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenCustomersController_WhenPostInvalidName_ThenBadRequest()
        {
            using var client = _application.CreateClient();

            var customer = new CustomerCreateDto()
            {
                Email = "new@test.com",
                FirstName = "",
                LastName = "Tester"
            };

            var response = await client.PostAsync(BASE_URL, _application.GetStringContent(customer));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenCustomersController_WhenPut_ThenUpdate()
        {
            using var client = _application.CreateClient();
            var customer = _application.TestCustomers.First();

            var update = new CustomerUpdateDto()
            {
                FirstName = "Updated",
                LastName = "Name"
            };

            var response = await client.PutAsync($"{BASE_URL}/{customer.Id}", _application.GetStringContent(update));

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            customer.Name.Should().Be(Name.Create("Updated", "Name"));
        }

        [Fact]
        public async Task GivenCustomersController_WhenDelete_ThenLeaveAndCloseAccounts()
        {
            using var client = _application.CreateClient();
            var customer = _application.TestCustomers.First();

            var response = await client.DeleteAsync($"{BASE_URL}/{customer.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            customer.HasLeft.Should().BeTrue();
            foreach(var account in _application.TestAccounts.Where(e => e.CustomerId == customer.Id))
            {
                account.Closed.Should().BeTrue();
            }
        }
    }
}
