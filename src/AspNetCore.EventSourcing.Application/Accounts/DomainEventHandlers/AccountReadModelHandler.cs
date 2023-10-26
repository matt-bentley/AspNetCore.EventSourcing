using AspNetCore.EventSourcing.Application.Abstractions.Repositories;
using AspNetCore.EventSourcing.Core.Accounts.DomainEvents;
using AspNetCore.EventSourcing.Core.Accounts.ReadModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AspNetCore.EventSourcing.Application.Accounts.DomainEventHandlers
{
    public sealed class AccountReadModelHandler :
        INotificationHandler<AccountOpenedDomainEvent>,
        INotificationHandler<AmountDepositedDomainEvent>,
        INotificationHandler<AmountWithdrawnDomainEvent>,
        INotificationHandler<AccountClosedDomainEvent>
    {
        private readonly IReadModelRepository<AccountReadModel> _repository;
        private readonly ILogger<AccountReadModelHandler> _logger;

        public AccountReadModelHandler(IReadModelRepository<AccountReadModel> repository,
            ILogger<AccountReadModelHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Task Handle(AccountOpenedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating AccountReadModel");
            var account = new AccountReadModel()
            {
                Id = notification.Id,
                AccountNumber = notification.AccountNumber,
                Balance = 0,
                CustomerId = notification.CustomerId,
                OpenedDate = notification.OpenedDate,
            };
            _repository.Insert(account);
            return Task.CompletedTask;
        }

        public async Task Handle(AmountDepositedDomainEvent notification, CancellationToken cancellationToken)
        {
            await UpdateBalanceAsync(notification.AccountId, notification.NewBalance);
        }

        public async Task Handle(AmountWithdrawnDomainEvent notification, CancellationToken cancellationToken)
        {
            await UpdateBalanceAsync(notification.AccountId, notification.NewBalance);
        }

        private async Task UpdateBalanceAsync(Guid id, decimal newBalance)
        {
            _logger.LogInformation("Updating AccountReadModel Balance");
            var account = await GetReadModelAsync(id);
            account!.Balance = newBalance;
        }

        public async Task Handle(AccountClosedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Closing AccountReadModel");
            var account = await GetReadModelAsync(notification.Id);
            account!.Closed = true;
            account.ClosedDate = notification.ClosedDate;
        }

        private async Task<AccountReadModel?> GetReadModelAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
