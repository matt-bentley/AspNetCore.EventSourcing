using AspNetCore.EventSourcing.Application.Abstractions.Queries;
using AspNetCore.EventSourcing.Application.Abstractions.Repositories;
using AspNetCore.EventSourcing.Core.Abstractions.Guards;
using AspNetCore.EventSourcing.Core.Accounts.ReadModels;

namespace AspNetCore.EventSourcing.Application.Accounts.Queries
{
    public sealed record GetAccountQuery(Guid Id): Query<AccountReadModel>;

    public sealed class GetAccountQueryHandler : QueryHandler<GetAccountQuery, AccountReadModel>
    {
        private readonly IReadModelRepository<AccountReadModel> _repository;

        public GetAccountQueryHandler(IReadModelRepository<AccountReadModel> repository) : base(null)
        {
            _repository = repository;
        }

        protected async override Task<AccountReadModel> HandleAsync(GetAccountQuery request)
        {
            var account = await _repository.GetByIdAsync(request.Id);
            return Guard.Against.NotFound(account);
        }
    }
}
