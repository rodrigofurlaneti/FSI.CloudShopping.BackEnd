using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Entities
{
    public class Company : Entity
    {
        public BusinessTaxId BusinessTaxId { get; private set; }
        public string CompanyName { get; private set; }
        public string? StateTaxId { get; private set; }
        public virtual Customer Customer { get; private set; }
        protected Company() { }
        public Company(int customerId, BusinessTaxId businessTaxId, string companyName, string? stateTaxId)
        {
            Id = customerId;
            BusinessTaxId = businessTaxId;
            CompanyName = companyName;
            StateTaxId = stateTaxId;
        }
        public void UpdateCompanyName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new DomainException("O nome da empresa não pode ser vazio.");

            CompanyName = newName;
        }
    }
}