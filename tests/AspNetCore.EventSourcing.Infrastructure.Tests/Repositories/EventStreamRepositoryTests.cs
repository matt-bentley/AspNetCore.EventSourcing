using AspNetCore.EventSourcing.Core.Accounts.Entities;
using AspNetCore.EventSourcing.Core.Accounts.ReadModels;
using AspNetCore.EventSourcing.Core.Customers.Entities;
using AspNetCore.EventSourcing.Core.Tests.Builders;
using AspNetCore.EventSourcing.Infrastructure.Tests.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.EventSourcing.Infrastructure.Tests.Repositories
{
    public class EventStreamRepositoryTests : BaseRepositoryTests
    {
        public EventStreamRepositoryTests() : base(false)
        {

        }

        [Fact]
        public async Task GivenAccount_WhenSaveAsync_ThenCanGetById()
        {
            var customer = await SeedCustomerAsync();

            var account = new AccountBuilder().WithCustomer(customer.Id).Build();
            account.Deposit(100m, "Deposited");
            await GetEventStreamRepository<Account>().SaveAsync(account);
            await GetUnitOfWork().CommitAsync();
            account = await GetEventStreamRepository<Account>().GetByIdAsync(account.Id);
            account.Should().NotBeNull();
            account.Balance.Should().Be(100m);
        }

        [Fact]
        public async Task GivenAccount_WhenSaveAsync_ThenAccountReadModelGenerated()
        {
            var customer = await SeedCustomerAsync();

            var account = new AccountBuilder().WithCustomer(customer.Id).Build();
            account.Deposit(100m, "Deposited");
            await GetEventStreamRepository<Account>().SaveAsync(account);
            await GetUnitOfWork().CommitAsync();

            var readModel = await GetReadModelRepository<AccountReadModel>().GetByIdAsync(account.Id);
            readModel.Should().NotBeNull();
            readModel!.Balance.Should().Be(100m);
        }

        [Fact]
        public async Task GivenAccount_WhenSaveAsync_ThenTransactionsGenerated()
        {
            var customer = await SeedCustomerAsync();

            var account = new AccountBuilder().WithCustomer(customer.Id).Build();
            account.Deposit(100m, "Deposited");
            account.Deposit(10m, "Withdrawn");
            await GetEventStreamRepository<Account>().SaveAsync(account);
            await GetUnitOfWork().CommitAsync();

            var transactions = await GetReadModelRepository<TransactionReadModel>()
                             .GetAll()
                             .Where(e => e.AccountId == account.Id)
                             .ToListAsync();

            transactions.Should().HaveCount(2);
        }

        private async Task<Customer> SeedCustomerAsync()
        {
            var customer = new CustomerBuilder().Build();
            var customerRepository = GetRepository<Customer>();
            customerRepository.Insert(customer);
            await GetUnitOfWork().CommitAsync();
            return customer;
        }
    }
}
