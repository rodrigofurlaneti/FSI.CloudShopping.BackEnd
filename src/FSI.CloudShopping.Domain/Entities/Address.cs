namespace FSI.CloudShopping.Domain.Entities;

using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Enums;
using FSI.CloudShopping.Domain.ValueObjects;

/// <summary>
/// Entity representing a customer address.
/// </summary>
public class Address : Entity<Guid>
{
    public Guid CustomerId { get; private set; }
    public AddressType AddressType { get; private set; }
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string? Complement { get; private set; }
    public string Neighborhood { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public ZipCode ZipCode { get; private set; }
    public string Country { get; private set; }
    public bool IsDefault { get; private set; }

    // Navigation
    public Customer? Customer { get; private set; }

    public Address(Guid id, Guid customerId, AddressType addressType, string street, string number,
        string neighborhood, string city, string state, ZipCode zipCode, string country = "BR", bool isDefault = false)
        : base(id)
    {
        CustomerId = customerId;
        AddressType = addressType;
        Street = street;
        Number = number;
        Neighborhood = neighborhood;
        City = city;
        State = state;
        ZipCode = zipCode;
        Country = country;
        IsDefault = isDefault;
    }

    protected Address() { }

    public void SetAsDefault()
    {
        IsDefault = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetNonDefault()
    {
        IsDefault = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string street, string number, string? complement, string neighborhood, string city, string state, ZipCode zipCode)
    {
        Street = street;
        Number = number;
        Complement = complement;
        Neighborhood = neighborhood;
        City = city;
        State = state;
        ZipCode = zipCode;
        UpdatedAt = DateTime.UtcNow;
    }
}
