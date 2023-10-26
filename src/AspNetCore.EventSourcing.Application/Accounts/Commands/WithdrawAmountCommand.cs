using AspNetCore.EventSourcing.Application.Abstractions.Commands;
using AspNetCore.EventSourcing.Application.Abstractions.Repositories;
using AspNetCore.EventSourcing.Core.Abstractions.Guards;
using AspNetCore.EventSourcing.Core.Accounts.Entities;

namespace AspNetCore.EventSourcing.Application.Accounts.Commands
{
    public sealed record WithdrawAmountCommand(Guid AccountId, decimal Amount, string? Description) : Command;

    public sealed class WithdrawAmountCommandHandler : CommandHandler<WithdrawAmountCommand>
    {
        private readonly IEventStreamRepository<Account> _repository;

        public WithdrawAmountCommandHandler(IUnitOfWork unitOfWork,
            IEventStreamRepository<Account> repository) : base(unitOfWork)
        {
            _repository = repository;
        }

        protected override async Task HandleAsync(WithdrawAmountCommand request)
        {
            var account = await _repository.GetByIdAsync(request.AccountId);
            account = Guard.Against.NotFound(account);

            account.Withdraw(request.Amount, request.Description);

            await _repository.SaveAsync(account);
            await UnitOfWork.CommitAsync();
        }
    }
}
