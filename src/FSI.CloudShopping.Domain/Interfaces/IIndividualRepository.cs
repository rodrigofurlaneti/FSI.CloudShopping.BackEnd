namespace FSI.CloudShopping.Domain.Interfaces;

using FSI.CloudShopping.Domain.Entities;

/// <summary>
/// Repository contract for Individual entity (B2C customers).
/// </summary>
public interface IIndividualRepository : IRepository<Individual, Guid>
{
    Task<Individual?> GetByTaxIdAsync(string taxId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Individual>> GetByBirthMonthAsync(int month, CancellationToken cancellationToken = default);
}