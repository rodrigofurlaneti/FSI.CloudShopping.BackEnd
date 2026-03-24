namespace FSI.CloudShopping.Domain.Entities;

using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;

/// <summary>
/// Entity representing a contact person for a customer (mainly for B2B).
/// </summary>
public class Contact : Entity<Guid>
{
    public Guid CustomerId { get; private set; }
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public Phone? Phone { get; private set; }
    public string? Position { get; private set; }
    public bool IsPrimary { get; private set; }

    // Navigation
    public Customer? Customer { get; private set; }

    public Contact(Guid id, Guid customerId, string name, Email email, Phone? phone = null, string? position = null, bool isPrimary = false)
        : base(id)
    {
        CustomerId = customerId;
        Name = name;
        Email = email;
        Phone = phone;
        Position = position;
        IsPrimary = isPrimary;
    }

    protected Contact() { }

    public void SetPrimary()
    {
        IsPrimary = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetNonPrimary()
    {
        IsPrimary = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string name, Email email, Phone? phone = null, string? position = null)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Position = position;
        UpdatedAt = DateTime.UtcNow;
    }
}
