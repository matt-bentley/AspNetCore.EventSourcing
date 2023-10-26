using AspNetCore.EventSourcing.Application.Accounts.Models;
using AspNetCore.EventSourcing.Core.Accounts.Entities;
using AspNetCore.EventSourcing.Core.Accounts.ReadModels;

namespace AspNetCore.EventSourcing.Api.Tests.Controllers
{
    public class AccountsControllerTests
    {
        private const string BASE_URL = "api/accounts";
        private readonly TestWebApplication _application = new TestWebApplication();

        [Fact]
        public async Task GivenAccountsController_WhenGet_ThenOk()
        {
            using var client = _application.CreateClient();
            var response = await client.GetAsync(BASE_URL);

            var Accounts = await response.ReadAndAssertSuccessAsync<List<AccountReadModel>>();

            Accounts.Should().HaveCount(_application.TestAccounts.Count);
        }

        [Fact]
        public async Task GivenAccountsController_WhenGet_ThenExcludeDeleted()
        {
            foreach(var account in _application.TestAccountReadModels)
            {
                account.Closed = true;
            }
            using var client = _application.CreateClient();
            var response = await client.GetAsync(BASE_URL);

            var Accounts = await response.ReadAndAssertSuccessAsync<List<AccountReadModel>>();

            Accounts.Should().HaveCount(0);
        }

        [Fact]
        public async Task GivenAccountsController_WhenGetById_ThenOk()
        {
            using var client = _application.CreateClient();
            var response = await client.GetAsync($"{BASE_URL}/{_application.TestAccounts.First().Id}");

            var Account = await response.ReadAndAssertSuccessAsync<AccountReadModel>();

            Account.Should().NotBeNull();
        }

        [Fact]
        public async Task GivenAccountsController_WhenPost_ThenCreated()
        {
            using var client = _application.CreateClient();

            var Account = new AccountCreateDto()
            {
                CustomerId = _application.TestCustomers.First().Id,
                AccountNumber = "xxxxxxxx"
            };

            var response = await client.PostAsync(BASE_URL, _application.GetStringContent(Account));

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            _application.AccountsRepository.Verify(e => e.SaveAsync(It.IsAny<Account>()), Times.Once);
            _application.UnitOfWork.Verify(e => e.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GivenAccountsController_WhenPostDuplicateAccountNumber_ThenBadRequest()
        {
            using var client = _application.CreateClient();

            var Account = new AccountCreateDto()
            {
                CustomerId = _application.TestCustomers.First().Id,
                AccountNumber = _application.TestAccounts.First().AccountNumber
            };

            var response = await client.PostAsync(BASE_URL, _application.GetStringContent(Account));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenAccountsController_WhenPostLeftCustomer_ThenBadRequest()
        {
            using var client = _application.CreateClient();
            var customer = _application.TestCustomers.First();
            customer.Leave();

            var Account = new AccountCreateDto()
            {
                CustomerId = customer.Id,
                AccountNumber = "xxxxxxxx"
            };

            var response = await client.PostAsync(BASE_URL, _application.GetStringContent(Account));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenAccountsController_WhenDeposit_ThenUpdateBalance()
        {
            using var client = _application.CreateClient();
            var account = _application.TestAccounts.First();
            var previousBalance = account.Balance;

            var transaction = new TransactionCreateDto()
            {
                Amount = 100,
                Description = "Test"
            };

            var response = await client.PutAsync($"{BASE_URL}/{account.Id}/deposit", _application.GetStringContent(transaction));

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            account.Balance.Should().Be(previousBalance + transaction.Amount);
        }

        [Fact]
        public async Task GivenAccountsController_WhenWithdraw_ThenUpdateBalance()
        {
            using var client = _application.CreateClient();
            var account = _application.TestAccounts.First();
            account.Deposit(1000, "Deposit");
            var previousBalance = account.Balance;

            var transaction = new TransactionCreateDto()
            {
                Amount = 10,
                Description = "Test"
            };

            var response = await client.PutAsync($"{BASE_URL}/{account.Id}/withdraw", _application.GetStringContent(transaction));

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            account.Balance.Should().Be(previousBalance - transaction.Amount);
        }

        [Fact]
        public async Task GivenAccountsController_WhenDelete_ThenLeaveAndCloseAccounts()
        {
            using var client = _application.CreateClient();
            var account = _application.TestAccounts.First();

            var response = await client.DeleteAsync($"{BASE_URL}/{account.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            account.Closed.Should().BeTrue();
        }
    }
}
