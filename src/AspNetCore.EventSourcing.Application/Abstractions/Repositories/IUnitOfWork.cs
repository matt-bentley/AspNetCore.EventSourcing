using AspNetCore.EventSourcing.Core.Abstractions.DomainEvents;

namespace AspNetCore.EventSourcing.Application.Abstractions.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> CommitAsync(CancellationToken cancellationToken = default);
        Task DispatchDomainEventsAsync(List<DomainEvent> domainEvents);
    }
}
