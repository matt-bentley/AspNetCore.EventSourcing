using AspNetCore.EventSourcing.Core.Abstractions.ReadModels;

namespace AspNetCore.EventSourcing.Application.Abstractions.Repositories
{
    public interface IReadModelRepository<T> : IRepositoryBase<T>
      where T : ReadModelBase
    {

    }
}
