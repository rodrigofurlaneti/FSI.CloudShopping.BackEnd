using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Application.Mappings
{
    public static class CustomerMapping
    {
        public static Customer ToEntity(this CustomerDTO dto)
        {
            if (dto == null) return null!;

            return new Customer(
                new Email(dto.Email),
                new TaxId(dto.TaxId),
                new Password(dto.Password)
            );
        }

        public static CustomerDTO ToDto(this Customer entity)
        {
            if (entity == null) return null!;

            return new CustomerDTO
            {
                Id = entity.Id,
                Email = entity.Email.Address,
                TaxId = entity.Document.Number,
                IsCompany = entity.Document.IsCompany,
                Name = entity.Contacts.FirstOrDefault()?.Name.FullName ?? "N/A",
                Phone = entity.Contacts.FirstOrDefault()?.Phone.Number ?? "N/A"
            };
        }
    }
}