using AspNetCore.EventSourcing.Core.Abstractions.Entities;
using AspNetCore.EventSourcing.Application.Abstractions.Repositories;
using AspNetCore.EventSourcing.Infrastructure.Repositories.Abstract;

namespace AspNetCore.EventSourcing.Infrastructure.Repositories
{
    internal class Repository<T> : RepositoryBase<T>, IRepository<T> 
        where T : AggregateRoot
    {
        public Repository(BankingContext context) : base(context)
        {

        }
    }
}
