using AspNetCore.EventSourcing.Application.Abstractions.DomainEventHandlers;
using AspNetCore.EventSourcing.Core.Abstractions.Services;
using AspNetCore.EventSourcing.Core.Accounts.DomainEvents;
using Microsoft.Extensions.Logging;

namespace AspNetCore.EventSourcing.Application.Accounts.DomainEventHandlers
{
    public sealed class LimitWarningDomainEventHandler : DomainEventHandler<AmountWithdrawnDomainEvent>
    {
        private readonly INotificationsService _notificationsService;
        private const decimal _warningLimit = 100m;

        public LimitWarningDomainEventHandler(INotificationsService notificationsService,
            ILogger<LimitWarningDomainEventHandler> logger) : base(logger)
        {
            _notificationsService = notificationsService;
        }

        protected async override Task OnHandleAsync(AmountWithdrawnDomainEvent @event)
        {
            if(@event.NewBalance < _warningLimit)
            {
                Logger.LogWarning("Balance below warning limit.");
                await _notificationsService.AccountLimitWarningAsync(@event.NewBalance, @event.AccountId);
            }
        }
    }
}
