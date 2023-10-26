using AspNetCore.EventSourcing.Application.Abstractions.Queries;
using AspNetCore.EventSourcing.Application.Abstractions.Repositories;
using AspNetCore.EventSourcing.Core.Abstractions.Guards;
using AspNetCore.EventSourcing.Core.Accounts.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.EventSourcing.Application.Accounts.Queries
{
    public sealed record GetTransactionsQuery(Guid AccountId): Query<List<TransactionReadModel>>;

    public sealed class GetTransactionsQueryHandler : QueryHandler<GetTransactionsQuery, List<TransactionReadModel>>
    {
        private readonly IReadModelRepository<TransactionReadModel> _repository;
        private readonly IReadModelRepository<AccountReadModel> _accountsRepository;

        public GetTransactionsQueryHandler(IReadModelRepository<TransactionReadModel> repository,
            IReadModelRepository<AccountReadModel> accountsRepository) : base(null)
        {
            _repository = repository;
            _accountsRepository = accountsRepository;
        }

        protected async override Task<List<TransactionReadModel>> HandleAsync(GetTransactionsQuery request)
        {
            var account = await _accountsRepository.GetByIdAsync(request.AccountId);
            Guard.Against.NotFound(account, $"Account not found: {request.AccountId}");

            return await _repository.GetAll()
                                    .Where(e => e.AccountId == request.AccountId)
                                    .OrderByDescending(e => e.TransationDate)
                                    .ToListAsync();
        }
    }
}
