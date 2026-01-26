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
            if (customer == null) throw new DomainException("Cliente não encontrado.");
            var address = Mapper.Map<Address>(dto);
            customer.AddAddress(address);
            await _addressRepository.AddAsync(address);
            await _addressRepository.SaveChangesAsync();
            return Mapper.Map<AddressDTO>(address);
        }

        public async Task SetDefaultAddressAsync(int addressId, int customerId)
        {
            var addresses = (await _addressRepository.GetByCustomerIdAsync(customerId)).ToList();
            var addressToSet = addresses.FirstOrDefault(a => a.Id == addressId);
            if (addressToSet == null)
                throw new DomainException("Endereço não encontrado ou não pertence ao cliente.");
            foreach (var addr in addresses)
            {
                if (addr.Id == addressId)
                    addr.SetAsDefault();
                else
                    addr.SetNonDefault();
                await _addressRepository.UpdateAsync(addr);
            }
            await _addressRepository.SaveChangesAsync();
        }
        public async Task<IEnumerable<AddressDTO>> GetByCustomerIdAsync(int customerId)
        {
            var addresses = await _addressRepository.GetByCustomerIdAsync(customerId);
            return Mapper.Map<IEnumerable<AddressDTO>>(addresses);
        }
    }
}