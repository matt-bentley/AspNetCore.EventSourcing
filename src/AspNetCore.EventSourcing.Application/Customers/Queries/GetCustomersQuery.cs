using AspNetCore.EventSourcing.Application.Abstractions.Queries;
using AspNetCore.EventSourcing.Application.Abstractions.Repositories;
using AspNetCore.EventSourcing.Application.Customers.Models;
using AspNetCore.EventSourcing.Core.Customers.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.EventSourcing.Application.Customers.Queries
{
    public sealed record GetCustomersQuery() : Query<List<CustomerDto>>;

    public sealed class GetCustomersQueryHandler : QueryHandler<GetCustomersQuery, List<CustomerDto>>
    {
        private readonly IRepository<Customer> _repository;

        public GetCustomersQueryHandler(IMapper mapper,
            IRepository<Customer> repository) : base(mapper)
        {
            _repository = repository;
        }

        protected override async Task<List<CustomerDto>> HandleAsync(GetCustomersQuery request)
        {
            var query = _repository.GetAll()
                                   .Where(e => !e.HasLeft);
                                             
            return await Mapper.ProjectTo<CustomerDto>(query).ToListAsync();
        }
    }
}
