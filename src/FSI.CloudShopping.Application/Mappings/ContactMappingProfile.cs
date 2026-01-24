using AutoMapper;
using FSI.CloudShopping.Application.DTOs.Contact;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;

namespace FSI.CloudShopping.Application.Mappings
{
    public class ContactMappingProfile : Profile
    {
        public ContactMappingProfile()
        {
            CreateMap<Contact, ContactDTO>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.FullName))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email.Address))
                .ForMember(d => d.Phone, o => o.MapFrom(s => s.Phone.Number));

            CreateMap<ContactDTO, Contact>()
                .ConstructUsing(d => new Contact(
                    d.CustomerId,
                    new PersonName(d.Name),
                    new Email(d.Email),
                    new Phone(d.Phone),
                    d.Position));
        }
    }
}