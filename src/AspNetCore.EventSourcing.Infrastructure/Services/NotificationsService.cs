using AspNetCore.EventSourcing.Core.Abstractions.Services;
using Microsoft.Extensions.Logging;

namespace AspNetCore.EventSourcing.Infrastructure.Services
{
    internal sealed class NotificationsService : INotificationsService
    {
        private readonly ILogger<NotificationsService> _logger;

        public NotificationsService(ILogger<NotificationsService> logger)
        {
            _logger = logger;
        }

        public Task AccountLimitWarningAsync(decimal balance, Guid accountId)
        {
            // This class is included for demonstration only
            // In a real app it would integrate with an SMTP server or messaging service
            _logger.LogInformation("Send Account Limit Warning Notification");
            return Task.CompletedTask;
        }
    }
}
