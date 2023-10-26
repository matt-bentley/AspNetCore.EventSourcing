using AspNetCore.EventSourcing.Application.Abstractions.Commands;
using AspNetCore.EventSourcing.Application.Abstractions.Repositories;
using AspNetCore.EventSourcing.Core.Abstractions.Guards;
using AspNetCore.EventSourcing.Core.Customers.Entities;
using AspNetCore.EventSourcing.Core.Customers.ValueObjects;

namespace AspNetCore.EventSourcing.Application.Customers.Commands
{
    public sealed record UpdateCustomerNameCommand(Guid Id, string? FirstName, string? LastName) : Command;

    public sealed class UpdateCustomerNameCommandHandler : CommandHandler<UpdateCustomerNameCommand>
    {
        private readonly IRepository<Customer> _repository;

        public UpdateCustomerNameCommandHandler(IUnitOfWork unitOfWork,
            IRepository<Customer> repository) : base(unitOfWork)
        {
            _repository = repository;
        }

        protected override async Task HandleAsync(UpdateCustomerNameCommand request)
        {
            var customer = await _repository.GetByIdAsync(request.Id);
            customer = Guard.Against.NotFound(customer);
            var name = Name.Create(request.FirstName, request.LastName);
            customer.UpdateName(name);
            await UnitOfWork.CommitAsync();
        }
    }
}
