using AspNetCore.EventSourcing.Application.Customers.Models;
using AspNetCore.EventSourcing.Core.Customers.Entities;
using AutoMapper;

namespace AspNetCore.EventSourcing.Application.Customers.MappingProfiles
{
    internal sealed class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>()
                .ForMember(e => e.FirstName, src => src.MapFrom(e => e.Name.FirstName))
                .ForMember(e => e.LastName, src => src.MapFrom(e => e.Name.LastName))
                .ForMember(e => e.Email, src => src.MapFrom(e => e.Email.Value));
        }
    }
}
