using Microsoft.Extensions.Logging;
using AspNetCore.EventSourcing.Core.Abstractions.Services;
using AspNetCore.EventSourcing.Application.Accounts.DomainEventHandlers;
using AspNetCore.EventSourcing.Core.Accounts.DomainEvents;

namespace AspNetCore.EventSourcing.Application.Tests.Accounts.DomainEventHandlers
{
    public class LimitWarningDomainEventHandlerTests
    {
        private readonly LimitWarningDomainEventHandler _handler;
        private readonly Mock<INotificationsService> _notificationsService = new Mock<INotificationsService>();
        private readonly Guid _accountId = Guid.NewGuid();

        public LimitWarningDomainEventHandlerTests()
        {
            _handler = new LimitWarningDomainEventHandler(_notificationsService.Object, Mock.Of<ILogger<LimitWarningDomainEventHandler>>());
        }

        [Fact]
        public async Task GivenLimitWarningDomainEvent_WhenUnderLimit_ThenSendAlert()
        {
            Func<Task> action = () => _handler.Handle(new AmountWithdrawnDomainEvent(_accountId, 50, "Withdraw", 110, 60, DateTime.UtcNow), default(CancellationToken));
            await action.Should().NotThrowAsync();
            _notificationsService.Verify(e => e.AccountLimitWarningAsync(60, _accountId), Times.Once);
        }

        [Fact]
        public async Task GivenLimitWarningDomainEvent_WhenNotUnderLimit_ThenDontSendAlert()
        {
            Func<Task> action = () => _handler.Handle(new AmountWithdrawnDomainEvent(_accountId, 50, "Withdraw", 1110, 1060, DateTime.UtcNow), default(CancellationToken));
            await action.Should().NotThrowAsync();
            _notificationsService.Verify(e => e.AccountLimitWarningAsync(It.IsAny<decimal>(), _accountId), Times.Never);
        }
    }
}
