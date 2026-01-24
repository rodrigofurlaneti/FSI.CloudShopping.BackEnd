using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Application.DTOs.Cart;
using FSI.CloudShopping.Application.DTOs.Customer;
namespace FSI.CloudShopping.Application.Interfaces
{
    public interface ICartAppService : IBaseAppService<CartDTO>
    {
        Task MergeAfterLoginAsync(Guid visitorToken, int customerId);
        Task ClearCartAsync(string token);
    }
}