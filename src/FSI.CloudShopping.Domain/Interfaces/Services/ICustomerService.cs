using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Interfaces.Services
{
    public interface ICustomerService
    {
        Task EnsureUniqueTaxIdAsync(TaxId document);
    }
}
