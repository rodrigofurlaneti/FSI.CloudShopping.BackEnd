namespace FSI.CloudShopping.Domain.Interfaces;

using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;

public interface ICustomerRepository : IRepository<Customer, Guid>
{
    Task<Customer?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
    Task<Customer?> GetBySessionTokenAsync(Guid sessionToken, CancellationToken cancellationToken = default);
    Task<Customer?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(Email email, CancellationToken cancellationToken = default);
}
