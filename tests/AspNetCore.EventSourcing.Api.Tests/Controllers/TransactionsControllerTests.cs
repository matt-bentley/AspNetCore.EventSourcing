using AspNetCore.EventSourcing.Core.Accounts.ReadModels;

namespace AspNetCore.EventSourcing.Api.Tests.Controllers
{
    public class TransactionsControllerTests
    {
        private readonly TestWebApplication _application = new TestWebApplication();

        [Fact]
        public async Task GivenTransactionsController_WhenGet_ThenOk()
        {
            using var client = _application.CreateClient();
            var response = await client.GetAsync($"api/accounts/{_application.TestAccounts.First().Id}/transactions");

            var Accounts = await response.ReadAndAssertSuccessAsync<List<TransactionReadModel>>();

            Accounts.Should().HaveCount(1);
        }
    }
}
