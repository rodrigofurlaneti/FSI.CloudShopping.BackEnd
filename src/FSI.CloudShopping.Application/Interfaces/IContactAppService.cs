using FSI.CloudShopping.Application.DTOs.Contact;
using FSI.CloudShopping.Application.Interfaces;
namespace FSI.CloudShopping.Application.Interfaces
{
    public interface IContactAppService : IBaseAppService<ContactDTO>
    {
        Task<IEnumerable<ContactDTO>> GetByCustomerIdAsync(int customerId);
    }
}