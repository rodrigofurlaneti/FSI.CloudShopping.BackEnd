namespace FSI.CloudShopping.Application.DTOs;

using FSI.CloudShopping.Domain.Enums;

public class CustomerDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public CustomerType Type { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public IndividualDto? Individual { get; set; }
    public CompanyDto? Company { get; set; }
    public List<AddressDto> Addresses { get; set; } = [];
    public List<ContactDto> Contacts { get; set; } = [];
}

public class IndividualDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
}

public class CompanyDto
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string BusinessTaxId { get; set; } = string.Empty;
    public string? StateTaxId { get; set; }
    public string? TradeName { get; set; }
}

public class AddressDto
{
    public Guid Id { get; set; }
    public string AddressType { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string? Complement { get; set; }
    public string Neighborhood { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
}

public class ContactDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Position { get; set; }
    public bool IsPrimary { get; set; }
}

public record CreateIndividualRequest(string Email, string Password, string FirstName, string LastName, string TaxId, DateTime BirthDate);

public record CreateCompanyRequest(string Email, string Password, string CompanyName, string BusinessTaxId, string? StateTaxId = null, string? TradeName = null);

public record UpdateCustomerRequest(string? Email = null);

public record CreateAddressRequest(string AddressType, string Street, string Number, string? Complement, string Neighborhood, string City, string State, string ZipCode, bool IsDefault = false);

public record UpdateAddressRequest(string Street, string Number, string? Complement, string Neighborhood, string City, string State, string ZipCode);

public record CreateContactRequest(string Name, string Email, string? Phone = null, string? Position = null, bool IsPrimary = false);

public record UpdateContactRequest(string Name, string Email, string? Phone = null, string? Position = null);
