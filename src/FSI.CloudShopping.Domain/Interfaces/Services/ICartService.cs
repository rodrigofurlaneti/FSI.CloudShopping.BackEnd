using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Interfaces.Services
{
    public interface ICartService
    {
        /// <summary>
        /// Transfere itens de um carrinho de visitante para um cliente logado.
        /// </summary>
        Task MergeCartAsync(VisitorToken token, int customerId);
    }
}
