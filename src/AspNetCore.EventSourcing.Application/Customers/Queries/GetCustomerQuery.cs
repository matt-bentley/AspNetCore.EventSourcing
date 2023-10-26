using AspNetCore.EventSourcing.Application.Abstractions.Queries;
using AspNetCore.EventSourcing.Application.Abstractions.Repositories;
using AspNetCore.EventSourcing.Application.Customers.Models;
using AspNetCore.EventSourcing.Core.Abstractions.Guards;
using AspNetCore.EventSourcing.Core.Customers.Entities;
using AutoMapper;

namespace AspNetCore.EventSourcing.Application.Customers.Queries
{
    public sealed record GetCustomerQuery(Guid CustomerId) : Query<CustomerDto>;

    public sealed class GetCustomerQueryHandler : QueryHandler<GetCustomerQuery, CustomerDto>
    {
        private readonly IRepository<Customer> _repository;

        public GetCustomerQueryHandler(IMapper mapper,
            IRepository<Customer> repository) : base(mapper)
        {
            _repository = repository;
        }

        protected override async Task<CustomerDto> HandleAsync(GetCustomerQuery request)
        {
            var customer = await _repository.GetByIdAsync(request.CustomerId);
            Guard.Against.NotFound(customer);
            return Mapper.Map<CustomerDto>(customer);
        }
    }
}
