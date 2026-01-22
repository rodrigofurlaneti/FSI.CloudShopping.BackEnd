using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;

namespace FSI.CloudShopping.Application.Services
{
    public class CustomerAppService : BaseAppService<CustomerDTO, Customer>, ICustomerAppService
    {
        // Reutilizamos o repositório da base para evitar erros de compilação
        public CustomerAppService(ICustomerRepository repository) : base(repository) { }

        public async Task<CustomerDTO> RegisterAsync(CustomerDTO dto)
        {
            var customer = MapToEntity(dto);

            // Lógica Sênior: O Phone vem no DTO e deve ser salvo no primeiro contato
            var primaryContact = new Contact(
                customer.Id,
                new PersonName(dto.Name),
                new Email(dto.Email),
                new Phone(dto.Phone),
                "Principal");

            customer.AddContact(primaryContact);

            await Repository.AddAsync(customer);
            await Repository.SaveChangesAsync();

            return dto with { Id = customer.Id, IsCompany = customer.Document.IsCompany };
        }

        public async Task UpdateStatusAsync(int id, bool active)
        {
            var customer = await Repository.GetByIdAsync(id);
            if (customer == null) throw new ApplicationException("Cliente não encontrado.");

            if (active) customer.Activate(); else customer.Deactivate();

            await Repository.UpdateAsync(customer);
            await Repository.SaveChangesAsync();
        }

        public async Task UpdateAddressAsync(int customerId, AddressDTO addressDto)
        {
            var customer = await Repository.GetByIdAsync(customerId);
            if (customer == null) throw new ApplicationException("Cliente não encontrado.");

            var address = new Address(
                addressDto.Street,
                addressDto.Number,
                addressDto.Neighborhood,
                addressDto.City,
                addressDto.State,
                addressDto.ZipCode,
                addressDto.IsDefault);

            customer.UpdateAddress(address);

            await Repository.UpdateAsync(customer);
            await Repository.SaveChangesAsync();
        }

        protected override Customer MapToEntity(CustomerDTO dto) =>
            new Customer(
                new Email(dto.Email),
                new TaxId(dto.TaxId),
                new Password(dto.Password)
            );

        protected override CustomerDTO MapToDto(Customer entity) =>
            new CustomerDTO
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