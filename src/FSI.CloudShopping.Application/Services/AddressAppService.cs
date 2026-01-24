using AutoMapper;
using FSI.CloudShopping.Application.DTOs.Address;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
namespace FSI.CloudShopping.Application.Services
{
    public class AddressAppService : BaseAppService<Address, AddressDTO>, IAddressAppService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly ICustomerRepository _customerRepository;

        public AddressAppService(
            IAddressRepository addressRepository,
            ICustomerRepository customerRepository,
            IMapper mapper) : base(addressRepository, mapper)
        {
            _addressRepository = addressRepository;
            _customerRepository = customerRepository;
        }
        public override async Task<AddressDTO> AddAsync(AddressDTO dto)
        {
            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
            if (customer == null) throw new DomainException("Cliente não encontrado para vincular o endereço.");
            var address = new Address(
                dto.CustomerId,
                dto.AddressType,
                dto.Street,
                dto.Number,
                dto.City,
                dto.State,
                dto.ZipCode,
                dto.IsDefault
            );
            customer.AddAddress(address);
            await _addressRepository.AddAsync(address);
            await _addressRepository.SaveChangesAsync();

            return Mapper.Map<AddressDTO>(address);
        }
        public async Task<IEnumerable<AddressDTO>> GetByCustomerIdAsync(int customerId)
        {
            var addresses = await _addressRepository.GetByCustomerIdAsync(customerId);
            return Mapper.Map<IEnumerable<AddressDTO>>(addresses);
        }
        public async Task SetDefaultAddressAsync(int addressId, int customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null) throw new DomainException("Cliente não encontrado.");
            var address = await _addressRepository.GetByIdAsync(addressId);
            if (address == null || address.CustomerId != customerId)
                throw new DomainException("Endereço não pertence ao cliente informado.");
            address.SetAsDefault();
            var otherAddresses = await _addressRepository.GetByCustomerIdAsync(customerId);
            foreach (var addr in otherAddresses.Where(a => a.Id != addressId))
            {
                addr.SetNonDefault();
                await _addressRepository.UpdateAsync(addr);
            }

            await _addressRepository.UpdateAsync(address);
            await _addressRepository.SaveChangesAsync();
        }
    }
}