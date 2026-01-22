using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Application.Mappings;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Application.Services
{
    public class CustomerAppService : BaseAppService<CustomerDTO, Customer>, ICustomerAppService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerAppService(ICustomerRepository repository) : base(repository)
        {
            _customerRepository = repository;
        }
        public async Task<CustomerDTO> RegisterAsync(CustomerDTO dto)
        {
            var customer = dto.ToEntity();
            var primaryContact = new Contact(
                customer.Id,
                new PersonName(dto.Name),
                new Email(dto.Email),
                new Phone(dto.Phone),
                "Principal");
            customer.AddContact(primaryContact);
            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveChangesAsync();
            return customer.ToDto();
        }
        public async Task UpdateStatusAsync(int id, bool active)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                throw new KeyNotFoundException($"Cliente com ID {id} não encontrado.");
            if (active) customer.Activate(); else customer.Deactivate();
            await _customerRepository.UpdateAsync(customer);
            await _customerRepository.SaveChangesAsync();
        }
        public async Task UpdateAddressAsync(int customerId, AddressDTO addressDto)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null)
                throw new KeyNotFoundException($"Cliente com ID {customerId} não encontrado.");
            var address = new Address(
                addressDto.Street,
                addressDto.Number,
                addressDto.Neighborhood,
                addressDto.City,
                addressDto.State,
                addressDto.ZipCode,
                addressDto.IsDefault);
            customer.UpdateAddress(address);
            await _customerRepository.UpdateAsync(customer);
            await _customerRepository.SaveChangesAsync();
        }
        protected override Customer MapToEntity(CustomerDTO dto) => dto.ToEntity();
        protected override CustomerDTO MapToDto(Customer entity) => entity.ToDto();
    }
}