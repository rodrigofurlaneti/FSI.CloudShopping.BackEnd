namespace FSI.CloudShopping.Domain.Interfaces;

using FSI.CloudShopping.Domain.Entities;

/// <summary>
/// Repository contract for Company entity (B2B customers).
/// </summary>
public interface ICompanyRepository : IRepository<Company, Guid>
{
    Task<Company?> GetByBusinessTaxIdAsync(string businessTaxId, CancellationToken cancellationToken = default);
    Task<Company?> GetByStateTaxIdAsync(string stateTaxId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Company>> SearchByCompanyNameAsync(string name, CancellationToken cancellationToken = default);
}
