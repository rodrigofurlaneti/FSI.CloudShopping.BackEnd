using FSI.CloudShopping.Domain.Entities;
namespace FSI.CloudShopping.Domain.Interfaces.Services
{
    public interface IOrderService
    {
        /// <summary>
        /// Inicia o processo de checkout e gera o pedido.
        /// </summary>
        Task<Order> PlaceOrderAsync(int cartId);
    }
}
