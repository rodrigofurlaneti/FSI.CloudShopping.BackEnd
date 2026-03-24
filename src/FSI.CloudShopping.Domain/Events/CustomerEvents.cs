namespace FSI.CloudShopping.Domain.Events;

using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Enums;

public class CustomerCreatedEvent : DomainEvent
{
    public Guid CustomerId { get; }
    public string Email { get; }
    public CustomerType Type { get; }

    public CustomerCreatedEvent(Guid customerId, string email, CustomerType type)
    {
        CustomerId = customerId;
        Email = email;
        Type = type;
    }
}

public class CustomerBecameLeadEvent : DomainEvent
{
    public Guid CustomerId { get; }
    public string Email { get; }

    public CustomerBecameLeadEvent(Guid customerId, string email)
    {
        CustomerId = customerId;
        Email = email;
    }
}

public class CustomerBecameB2CEvent : DomainEvent
{
    public Guid CustomerId { get; }
    public string Email { get; }
    public string FullName { get; }
    public string TaxId { get; }

    public CustomerBecameB2CEvent(Guid customerId, string email, string fullName, string taxId)
    {
        CustomerId = customerId;
        Email = email;
        FullName = fullName;
        TaxId = taxId;
    }
}

public class CustomerBecameB2BEvent : DomainEvent
{
    public Guid CustomerId { get; }
    public string Email { get; }
    public string CompanyName { get; }
    public string BusinessTaxId { get; }

    public CustomerBecameB2BEvent(Guid customerId, string email, string companyName, string businessTaxId)
    {
        CustomerId = customerId;
        Email = email;
        CompanyName = companyName;
        BusinessTaxId = businessTaxId;
    }
}
