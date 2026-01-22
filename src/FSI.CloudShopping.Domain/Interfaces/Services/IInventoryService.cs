using FSI.CloudShopping.Domain.Entities;
namespace FSI.CloudShopping.Domain.Interfaces.Services
{
    public interface IInventoryService
    {
        /// <summary>
        /// Valida se há estoque disponível e realiza a reserva.
        /// </summary>
        Task ValidateAndReserveStockAsync(IEnumerable<CartItem> items);
    }
}
