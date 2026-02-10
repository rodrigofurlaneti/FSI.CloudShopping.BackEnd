using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Application.DTOs.Cart;
using FSI.CloudShopping.Application.DTOs.Customer;
namespace FSI.CloudShopping.Application.Interfaces
{
    public interface ICartAppService : IBaseAppService<CartDTO>
    {
        Task<CartDTO> GetByTokenAsync(Guid sessionToken); 
        Task<CartDTO> AddItemAsync(Guid sessionToken, int productId, int quantity);
        Task MergeAfterLoginAsync(Guid visitorToken, int customerId);
        Task ClearCartAsync(string token);
    }
}