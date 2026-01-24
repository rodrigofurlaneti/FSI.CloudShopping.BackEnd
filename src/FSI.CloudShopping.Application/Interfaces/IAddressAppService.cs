using FSI.CloudShopping.Application.DTOs.Address;
namespace FSI.CloudShopping.Application.Interfaces
{
    public interface IAddressAppService : IBaseAppService<AddressDTO>
    {
        Task<IEnumerable<AddressDTO>> GetByCustomerIdAsync(int customerId);
        Task SetDefaultAddressAsync(int addressId, int customerId);
    }
}