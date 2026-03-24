namespace FSI.CloudShopping.Domain.Entities;

using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;

/// <summary>
/// Entity representing a B2B (company) customer.
/// </summary>
public class Company : Entity<Guid>
{
    public Guid CustomerId { get; private set; }
    public BusinessTaxId BusinessTaxId { get; private set; }
    public string CompanyName { get; private set; }
    public string? StateTaxId { get; private set; }
    public string? TradeName { get; private set; }

    // Navigation
    public Customer? Customer { get; private set; }

    public Company(Guid id, Guid customerId, BusinessTaxId businessTaxId, string companyName, string? stateTaxId = null, string? tradeName = null) : base(id)
    {
        CustomerId = customerId;
        BusinessTaxId = businessTaxId;
        CompanyName = companyName;
        StateTaxId = stateTaxId;
        TradeName = tradeName;
    }

    protected Company() { }

    public void UpdateProfile(string companyName, string? stateTaxId = null, string? tradeName = null)
    {
        CompanyName = companyName;
        StateTaxId = stateTaxId;
        TradeName = tradeName;
        UpdatedAt = DateTime.UtcNow;
    }
}
