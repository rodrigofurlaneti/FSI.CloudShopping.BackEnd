using AutoMapper;
using FSI.CloudShopping.Application.DTOs.Customer;
using FSI.CloudShopping.Domain.Entities;
namespace FSI.CloudShopping.Application.Mappings
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<Customer, CustomerDTO>()
                .ForMember(d => d.CustomerType, o => o.MapFrom(s => s.CustomerType.Code))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.Individual != null ? s.Individual.FullName.FullName : null))
                .ForMember(d => d.Document, o => o.MapFrom(s => s.Individual != null ? s.Individual.TaxId.Number : (s.Company != null ? s.Company.BusinessTaxId.Number : null)))
                .ForMember(d => d.CompanyName, o => o.MapFrom(s => s.Company != null ? s.Company.CompanyName : null));
        }
    }
}