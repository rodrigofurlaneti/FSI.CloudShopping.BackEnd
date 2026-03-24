namespace FSI.CloudShopping.Domain.Entities;

using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Enums;
using FSI.CloudShopping.Domain.ValueObjects;

/// <summary>
/// Aggregate root for Customer. Represents a customer in the e-commerce system.
/// A customer can be a Guest, Lead, B2C (Individual), or B2B (Company).
/// </summary>
public class Customer : AggregateRoot<Guid>
{
    public Email Email { get; private set; }
    public Password? Password { get; private set; }
    public CustomerType Type { get; private set; }
    public Guid SessionToken { get; private set; }
    public bool IsActive { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiry { get; private set; }
    public string? GeoLocation { get; private set; }

    // Navigations
    public Individual? Individual { get; private set; }
    public Company? Company { get; private set; }
    private readonly List<Address> _addresses = [];
    public IReadOnlyCollection<Address> Addresses => _addresses.AsReadOnly();

    private readonly List<Contact> _contacts = [];
    public IReadOnlyCollection<Contact> Contacts => _contacts.AsReadOnly();

    public Customer(Guid id, Email email, CustomerType type) : base(id)
    {
        Email = email;
        Type = type;
        SessionToken = Guid.NewGuid();
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    protected Customer() { }

    public static Customer CreateGuest(Email email)
    {
        var customer = new Customer(Guid.NewGuid(), email, CustomerType.Guest);
        customer.RaiseDomainEvent(new CustomerCreatedEvent(customer.Id, customer.Email.Value, CustomerType.Guest));
        return customer;
    }

    public static Customer CreateLead(Email email)
    {
        var customer = new Customer(Guid.NewGuid(), email, CustomerType.Lead);
        customer.RaiseDomainEvent(new CustomerCreatedEvent(customer.Id, customer.Email.Value, CustomerType.Lead));
        return customer;
    }

    public void BecomeLead()
    {
        if (Type != CustomerType.Guest)
            throw new DomainException("Only guest customers can become leads.");

        Type = CustomerType.Lead;
        UpdatedAt = DateTime.UtcNow;
        RaiseDomainEvent(new CustomerBecameLeadEvent(Id, Email.Value));
    }

    public void BecomeIndividual(PersonName fullName, TaxId taxId, DateTime birthDate)
    {
        if (Type != CustomerType.Lead && Type != CustomerType.B2C)
            throw new DomainException("Only leads or existing B2C can become individual customers.");

        if (Individual == null)
        {
            Individual = new Individual(Guid.NewGuid(), Id, taxId, fullName, birthDate);
        }

        Type = CustomerType.B2C;
        UpdatedAt = DateTime.UtcNow;
        RaiseDomainEvent(new CustomerBecameB2CEvent(Id, Email.Value, fullName.FullName, taxId.Value));
    }

    public void BecomeCompany(string companyName, BusinessTaxId businessTaxId, string? stateTaxId = null, string? tradeName = null)
    {
        if (Type != CustomerType.Lead && Type != CustomerType.B2B)
            throw new DomainException("Only leads or existing B2B can become company customers.");

        if (Company == null)
        {
            Company = new Company(Guid.NewGuid(), Id, businessTaxId, companyName, stateTaxId, tradeName);
        }

        Type = CustomerType.B2B;
        UpdatedAt = DateTime.UtcNow;
        RaiseDomainEvent(new CustomerBecameB2BEvent(Id, Email.Value, companyName, businessTaxId.Value));
    }

    public void SetPassword(Password password)
    {
        Password = password;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ResetPassword(Password newPassword)
    {
        Password = newPassword;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddAddress(Address address)
    {
        if (_addresses.Any(a => a.AddressType == address.AddressType && a.IsDefault))
        {
            var defaultAddress = _addresses.FirstOrDefault(a => a.AddressType == address.AddressType && a.IsDefault);
            if (defaultAddress != null)
            {
                defaultAddress.SetNonDefault();
            }
        }

        _addresses.Add(address);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveAddress(Guid addressId)
    {
        var address = _addresses.FirstOrDefault(a => a.Id == addressId);
        if (address != null)
        {
            _addresses.Remove(address);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void AddContact(Contact contact)
    {
        if (_contacts.Any(c => c.IsPrimary))
        {
            var primaryContact = _contacts.First(c => c.IsPrimary);
            if (contact.IsPrimary)
            {
                primaryContact.SetNonPrimary();
            }
        }

        _contacts.Add(contact);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveContact(Guid contactId)
    {
        var contact = _contacts.FirstOrDefault(c => c.Id == contactId);
        if (contact != null)
        {
            _contacts.Remove(contact);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void UpdateRefreshToken(string token, DateTime expiry)
    {
        RefreshToken = token;
        RefreshTokenExpiry = expiry;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RevokeRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiry = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetGeoLocation(string geoLocation)
    {
        GeoLocation = geoLocation;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RefreshSessionToken()
    {
        SessionToken = Guid.NewGuid();
        UpdatedAt = DateTime.UtcNow;
    }
}
