using AspNetCore.EventSourcing.Application.Abstractions.Commands;
using AspNetCore.EventSourcing.Application.Abstractions.Repositories;
using AspNetCore.EventSourcing.Core.Abstractions.Guards;
using AspNetCore.EventSourcing.Core.Accounts.Entities;

namespace AspNetCore.EventSourcing.Application.Accounts.Commands
{
    public sealed record DepositAmountCommand(Guid AccountId, decimal Amount, string? Description) : Command;

    public sealed class DepositAmountCommandHandler : CommandHandler<DepositAmountCommand>
    {
        private readonly IEventStreamRepository<Account> _repository;

        public DepositAmountCommandHandler(IUnitOfWork unitOfWork,
            IEventStreamRepository<Account> repository) : base(unitOfWork)
        {
            _repository = repository;
        }

        protected override async Task HandleAsync(DepositAmountCommand request)
        {
            var account = await _repository.GetByIdAsync(request.AccountId);
            account = Guard.Against.NotFound(account);

            account.Deposit(request.Amount, request.Description);

            await _repository.SaveAsync(account);
            await UnitOfWork.CommitAsync();
        }
    }
}
