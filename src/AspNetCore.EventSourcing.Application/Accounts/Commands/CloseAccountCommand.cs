using AspNetCore.EventSourcing.Application.Abstractions.Commands;
using AspNetCore.EventSourcing.Application.Abstractions.Repositories;
using AspNetCore.EventSourcing.Core.Abstractions.Guards;
using AspNetCore.EventSourcing.Core.Accounts.Entities;

namespace AspNetCore.EventSourcing.Application.Accounts.Commands
{
    public sealed record CloseAccountCommand(Guid Id) : Command;

    public sealed class CloseAccountCommandHandler : CommandHandler<CloseAccountCommand>
    {
        private readonly IEventStreamRepository<Account> _repository;

        public CloseAccountCommandHandler(IUnitOfWork unitOfWork,
            IEventStreamRepository<Account> repository) : base(unitOfWork)
        {
            _repository = repository;
        }

        protected override async Task HandleAsync(CloseAccountCommand request)
        {
            var account = await _repository.GetByIdAsync(request.Id);
            account = Guard.Against.NotFound(account);

            account.Close();

            await _repository.SaveAsync(account);
            await UnitOfWork.CommitAsync();
        }
    }
}
