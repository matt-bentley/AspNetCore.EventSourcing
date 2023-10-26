using AspNetCore.EventSourcing.Core.Abstractions.Entities;

namespace AspNetCore.EventSourcing.Application.Abstractions.Repositories
{
    public interface IRepository<T> : IRepositoryBase<T> 
        where T : AggregateRoot
    {

    }
}
