using FSI.CloudShopping.Application.DTOs;
namespace FSI.CloudShopping.Application.Interfaces
{
    public interface ICustomerAppService : IBaseAppService<CustomerDTO>
    {
        Task<CustomerDTO> RegisterAsync(CustomerDTO customerDto);
        Task<CustomerDTO> GetByIdAsync(int id);
        Task UpdateAddressAsync(int customerId, AddressDTO addressDto);
        Task UpdateStatusAsync(int id, bool active);
    }
}
