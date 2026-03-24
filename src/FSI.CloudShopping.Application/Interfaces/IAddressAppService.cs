using FSI.CloudShopping.Application.DTOs.Address;
namespace FSI.CloudShopping.Application.Interfaces
{
    public interface IAddressAppService : IBaseAppService<AddressDTO>
    {
        Task<IEnumerable<AddressDTO>> GetByCustomerIdAsync(Guid customerId);
        Task SetDefaultAddressAsync(Guid addressId, Guid customerId);
    }
}