using AspNetCore.EventSourcing.Application.Abstractions.Commands;
using AspNetCore.EventSourcing.Application.Abstractions.Repositories;
using AspNetCore.EventSourcing.Core.Abstractions.Guards;
using AspNetCore.EventSourcing.Core.Accounts.Entities;
using AspNetCore.EventSourcing.Core.Accounts.ReadModels;
using AspNetCore.EventSourcing.Core.Customers.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AspNetCore.EventSourcing.Application.Customers.Commands
{
    public sealed record CustomerLeaveCommand(Guid Id) : Command;

    public sealed class CustomerLeaveCommandHandler : CommandHandler<CustomerLeaveCommand>
    {
        private readonly IRepository<Customer> _repository;
        private readonly IReadModelRepository<AccountReadModel> _accountReadModelsRepository;
        private readonly IEventStreamRepository<Account> _accountsRepository;
        private readonly ILogger<CustomerLeaveCommandHandler> _logger;

        public CustomerLeaveCommandHandler(IUnitOfWork unitOfWork,
            IRepository<Customer> repository,
            IReadModelRepository<AccountReadModel> accountReadModelsRepository,
            IEventStreamRepository<Account> accountsRepository,
            ILogger<CustomerLeaveCommandHandler> logger) : base(unitOfWork)
        {
            _repository = repository;
            _accountReadModelsRepository = accountReadModelsRepository;
            _accountsRepository = accountsRepository;
            _logger = logger;
        }

        protected override async Task HandleAsync(CustomerLeaveCommand request)
        {
            var customer = await _repository.GetByIdAsync(request.Id);
            customer = Guard.Against.NotFound(customer);
            customer.Leave();
            await CloseCustomerAccountsAsync(customer.Id);
            await UnitOfWork.CommitAsync();
        }

        private async Task CloseCustomerAccountsAsync(Guid customerId)
        {
            var accountIds = await _accountReadModelsRepository.GetAll()
                                                             .Where(e => e.CustomerId == customerId && !e.Closed)
                                                             .Select(e => e.Id)
                                                             .ToListAsync();
            foreach (var accountId in accountIds)
            {
                _logger.LogInformation("Closing account: {accountId}", accountId);
                var account = await _accountsRepository.GetByIdAsync(accountId);
                account.Close();
                await _accountsRepository.SaveAsync(account);
            }
        }
    }
}
