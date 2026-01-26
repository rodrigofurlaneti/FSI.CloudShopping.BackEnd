using AutoMapper;
using FSI.CloudShopping.Application.DTOs.Address;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.CloudShopping.Application.Mappings
{
    public class AddressMappingProfile : Profile
    {
        public AddressMappingProfile()
        {
            CreateMap<Address, AddressDTO>()
                .ConstructUsing(src => new AddressDTO(
                    src.Id,
                    src.CustomerId,
                    src.AddressType.Description, 
                    src.Street,
                    src.Number,
                    src.City,
                    src.State,
                    src.ZipCode,
                    src.Neighborhood,
                    src.IsDefault
                ));

            CreateMap<AddressDTO, Address>()
                .ConstructUsing(dto => new Address(
                    dto.CustomerId,
                    AddressType.FromString(dto.AddressType), 
                    dto.Street,
                    dto.Number,
                    dto.City,
                    dto.State,
                    dto.ZipCode,
                    dto.Neighborhood,
                    dto.IsDefault
                ));
        }
    }
}