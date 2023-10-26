using AspNetCore.EventSourcing.Application.Abstractions.Commands;
using AspNetCore.EventSourcing.Application.Abstractions.Repositories;
using AspNetCore.EventSourcing.Core.Abstractions.Exceptions;
using AspNetCore.EventSourcing.Core.Abstractions.Guards;
using AspNetCore.EventSourcing.Core.Accounts.Entities;
using AspNetCore.EventSourcing.Core.Accounts.ReadModels;
using AspNetCore.EventSourcing.Core.Customers.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.EventSourcing.Application.Accounts.Commands
{
    public sealed record OpenAccountCommand(Guid Id, Guid CustomerId, string? AccountNumber) : Command;

    public sealed class OpenAccountCommandHandler : CommandHandler<OpenAccountCommand>
    {
        private readonly IReadModelRepository<AccountReadModel> _readModelsRepository;
        private readonly IEventStreamRepository<Account> _repository;
        private readonly IRepository<Customer> _customersRepository;

        public OpenAccountCommandHandler(IUnitOfWork unitOfWork,
            IReadModelRepository<AccountReadModel> readModelsRepository,
            IEventStreamRepository<Account> repository,
            IRepository<Customer> customersRepository) : base(unitOfWork)
        {
            _readModelsRepository = readModelsRepository;
            _repository = repository;
            _customersRepository = customersRepository;
        }

        protected override async Task HandleAsync(OpenAccountCommand request)
        {
            var customer = await _customersRepository.GetByIdAsync(request.CustomerId);
            customer = Guard.Against.NotFound(customer, $"Customer not found: {request.CustomerId}");
            Guard.Against.CustomerHasLeft(customer);

            var account = Account.Create(request.Id, request.AccountNumber, request.CustomerId);
            await CheckForExistingAccountAsync(account.AccountNumber);
         
            await _repository.SaveAsync(account);
            await UnitOfWork.CommitAsync();
        }

        private async Task CheckForExistingAccountAsync(string accountNumber)
        {
            var existingAccount = await _readModelsRepository.GetAll()
                                                             .FirstOrDefaultAsync(e => e.AccountNumber == accountNumber);

            if (existingAccount != null)
            {
                throw new DomainException($"Account already exists: {accountNumber}");
            }
        }
    }
}
