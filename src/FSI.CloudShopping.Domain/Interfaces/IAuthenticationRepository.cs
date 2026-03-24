namespace FSI.CloudShopping.Domain.Interfaces;

using FSI.CloudShopping.Domain.Entities;

/// <summary>
/// Repository contract for Authentication entity.
/// </summary>
public interface IAuthenticationRepository : IRepository<Authentication, int>
{
    Task<int> InsertAsync(string email, bool isAuthorized, CancellationToken cancellationToken = default);
    Task<bool> GetAccessAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<Authentication?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}