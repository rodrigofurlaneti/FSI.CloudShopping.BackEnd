using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Customer?> GetByEmailAsync(Email email);
        Task<Customer?> GetBySessionTokenAsync(Guid token);
        Task<IEnumerable<Customer>> GetAllCompaniesAsync();
    }
}