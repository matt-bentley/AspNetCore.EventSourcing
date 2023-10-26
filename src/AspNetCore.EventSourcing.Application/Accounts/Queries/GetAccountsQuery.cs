using AspNetCore.EventSourcing.Application.Abstractions.Queries;
using AspNetCore.EventSourcing.Application.Abstractions.Repositories;
using AspNetCore.EventSourcing.Core.Accounts.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.EventSourcing.Application.Accounts.Queries
{
    public sealed record GetAccountsQuery(Guid? CustomerId): Query<List<AccountReadModel>>;

    public sealed class GetAccountsQueryHandler : QueryHandler<GetAccountsQuery, List<AccountReadModel>>
    {
        private readonly IReadModelRepository<AccountReadModel> _repository;

        public GetAccountsQueryHandler(IReadModelRepository<AccountReadModel> repository) : base(null)
        {
            _repository = repository;
        }

        protected async override Task<List<AccountReadModel>> HandleAsync(GetAccountsQuery request)
        {
            var query = _repository.GetAll()
                                   .Where(e => !e.Closed);

            if (request.CustomerId.HasValue)
            {
                query = query.Where(e => e.CustomerId == request.CustomerId.Value);
            }

            return await query.ToListAsync();
        }
    }
}
