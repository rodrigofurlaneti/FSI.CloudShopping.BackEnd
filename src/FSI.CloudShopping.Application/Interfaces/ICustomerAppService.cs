using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Application.DTOs.Customer;

namespace FSI.CloudShopping.Application.Interfaces
{
    public interface ICustomerAppService : IBaseAppService<CustomerDTO>
    {
        Task<CreateGuestResponse?> CreateGuestAsync(CreateGuestRequest request);
        Task RegisterLeadAsync(RegisterLeadRequest request);
        Task<CustomerDTO?> GetByEmailAsync(string email);
        Task UpdateToIndividualAsync(RegisterIndividualRequest request);
        Task UpdateToCompanyAsync(RegisterCompanyRequest request);
    }
}