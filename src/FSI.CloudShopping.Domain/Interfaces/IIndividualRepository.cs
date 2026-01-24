using FSI.CloudShopping.Domain.Entities;
namespace FSI.CloudShopping.Domain.Interfaces
{
    public interface IIndividualRepository : IRepository<Individual>
    {
        Task<Individual?> GetByTaxIdAsync(string taxId);
        Task<IEnumerable<Individual>> GetByBirthMonthAsync(int month);
    }
}