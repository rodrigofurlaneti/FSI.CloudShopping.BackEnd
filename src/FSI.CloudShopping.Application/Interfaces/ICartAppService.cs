using FSI.CloudShopping.Application.DTOs;
namespace FSI.CloudShopping.Application.Interfaces
{
    public interface ICartAppService
    {
        Task<CartDTO> GetCartAsync(string visitorTokenOrCustomerId);
        Task AddItemAsync(string token, AddItemDTO item);
        Task RemoveItemAsync(string token, int productId);
        Task MergeAfterLoginAsync(Guid visitorToken, int customerId);
        Task ClearCartAsync(string token);
    }
}