using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Interfaces.Services
{
    public interface ICustomerService
    {
        /// <summary>
        /// Garante que não existam documentos duplicados no sistema.
        /// </summary>
        Task EnsureUniqueTaxIdAsync(TaxId document);
    }
}
