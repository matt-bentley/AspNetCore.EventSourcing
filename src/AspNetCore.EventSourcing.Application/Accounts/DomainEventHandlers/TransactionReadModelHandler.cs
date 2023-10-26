using AspNetCore.EventSourcing.Application.Abstractions.Repositories;
using AspNetCore.EventSourcing.Core.Accounts.DomainEvents;
using AspNetCore.EventSourcing.Core.Accounts.ReadModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AspNetCore.EventSourcing.Application.Accounts.DomainEventHandlers
{
    public sealed class TransactionReadModelHandler :
        INotificationHandler<AmountDepositedDomainEvent>,
        INotificationHandler<AmountWithdrawnDomainEvent>
    {
        private readonly IReadModelRepository<TransactionReadModel> _repository;
        private readonly ILogger<TransactionReadModelHandler> _logger;

        public TransactionReadModelHandler(IReadModelRepository<TransactionReadModel> repository,
            ILogger<TransactionReadModelHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Task Handle(AmountDepositedDomainEvent notification, CancellationToken cancellationToken)
        {
            Create(notification.AccountId, notification.Amount, notification.Description, notification.NewBalance, notification.Date);
            return Task.CompletedTask;
        }

        public Task Handle(AmountWithdrawnDomainEvent notification, CancellationToken cancellationToken)
        {
            Create(notification.AccountId, notification.Amount, notification.Description, notification.NewBalance, notification.Date);
            return Task.CompletedTask;
        }

        private void Create(Guid accountId, decimal amount, string description, decimal balance, DateTime date)
        {
            _logger.LogInformation("Creating TransactionReadModel");
            var transaction = new TransactionReadModel()
            {
                Id = Guid.NewGuid(),
                AccountId = accountId,
                Amount = amount,
                Description = description,
                Balance = balance,
                TransationDate = date
            };
            _repository.Insert(transaction);
        }
    }
}
