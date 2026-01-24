using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer?> GetByEmailAsync(Email email);
        Task<Customer?> GetByIndividualDocumentAsync(string cpf);
        Task<Customer?> GetByCompanyDocumentAsync(string cnpj);
    }
}
