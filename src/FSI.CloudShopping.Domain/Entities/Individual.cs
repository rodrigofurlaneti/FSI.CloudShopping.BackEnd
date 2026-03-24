namespace FSI.CloudShopping.Domain.Entities;

using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;

/// <summary>
/// Entity representing a B2C (individual/person) customer.
/// </summary>
public class Individual : Entity<Guid>
{
    public Guid CustomerId { get; private set; }
    public TaxId TaxId { get; private set; }
    public PersonName FullName { get; private set; }
    public DateTime BirthDate { get; private set; }

    // Navigation
    public Customer? Customer { get; private set; }

    public Individual(Guid id, Guid customerId, TaxId taxId, PersonName fullName, DateTime birthDate) : base(id)
    {
        CustomerId = customerId;
        TaxId = taxId;
        FullName = fullName;
        BirthDate = birthDate;
    }

    protected Individual() { }

    public void UpdateProfile(PersonName fullName, DateTime birthDate)
    {
        FullName = fullName;
        BirthDate = birthDate;
        UpdatedAt = DateTime.UtcNow;
    }
}
