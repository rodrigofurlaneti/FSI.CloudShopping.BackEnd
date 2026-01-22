using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Interfaces.Services;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task EnsureUniqueTaxIdAsync(TaxId document)
        {
            var existing = await _customerRepository.GetByDocumentAsync(document);

            if (existing is not null)
                throw new DomainException("Este CPF/CNPJ já se encontra registado no sistema.");
        }
    }
}
