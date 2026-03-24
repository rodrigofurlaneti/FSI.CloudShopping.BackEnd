namespace FSI.CloudShopping.Domain.Interfaces;

using FSI.CloudShopping.Domain.Entities;

public interface ICartRepository : IRepository<Cart, int>
{
    Task<Cart?> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<Cart?> GetBySessionTokenAsync(Guid sessionToken, CancellationToken cancellationToken = default);
    Task DeleteExpiredAsync(CancellationToken cancellationToken = default);
}
