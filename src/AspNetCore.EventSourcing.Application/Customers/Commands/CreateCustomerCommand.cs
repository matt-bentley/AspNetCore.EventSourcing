using AspNetCore.EventSourcing.Application.Abstractions.Commands;
using AspNetCore.EventSourcing.Application.Abstractions.Repositories;
using AspNetCore.EventSourcing.Core.Abstractions.Exceptions;
using AspNetCore.EventSourcing.Core.Customers.Entities;
using AspNetCore.EventSourcing.Core.Customers.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.EventSourcing.Application.Customers.Commands
{
    public sealed record CreateCustomerCommand(Guid Id, string? FirstName, string? LastName, string? Email) : Command;

    public sealed class CreateCustomerCommandHandler : CommandHandler<CreateCustomerCommand>
    {
        private readonly IRepository<Customer> _repository;

        public CreateCustomerCommandHandler(IUnitOfWork unitOfWork,
            IRepository<Customer> repository) : base(unitOfWork)
        {
            _repository = repository;
        }

        protected override async Task HandleAsync(CreateCustomerCommand request)
        {
            var email = Email.Create(request.Email);
            var customer = await _repository.GetAll().FirstOrDefaultAsync(e => e.Email.Value == email.Value);
            if(customer != null)
            {
                throw new DomainException($"Customer already exists for email: {request.Email}");
            }
            var name = Name.Create(request.FirstName, request.LastName);
            customer = Customer.Create(request.Id, name, email);
            _repository.Insert(customer);
            await UnitOfWork.CommitAsync();
        }
    }
}
