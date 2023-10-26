
namespace AspNetCore.EventSourcing.Core.Abstractions.Services
{
    public interface INotificationsService
    {
        Task AccountLimitWarningAsync(decimal balance, Guid accountId);
    }
}
