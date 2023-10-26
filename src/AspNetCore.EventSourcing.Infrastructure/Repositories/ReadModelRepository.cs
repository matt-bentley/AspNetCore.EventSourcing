using AspNetCore.EventSourcing.Application.Abstractions.Repositories;
using AspNetCore.EventSourcing.Core.Abstractions.ReadModels;
using AspNetCore.EventSourcing.Infrastructure.Repositories.Abstract;

namespace AspNetCore.EventSourcing.Infrastructure.Repositories
{
    internal class ReadModelRepository<T> : RepositoryBase<T>, IReadModelRepository<T>
        where T : ReadModelBase
    {
        public ReadModelRepository(BankingContext context) : base(context)
        {
        }
    }
}
