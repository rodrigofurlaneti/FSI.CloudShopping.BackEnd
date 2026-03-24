namespace FSI.CloudShopping.Domain.ValueObjects;

using FSI.CloudShopping.Domain.Core;

/// <summary>
/// Value object representing a person's name with first and last name.
/// </summary>
public class PersonName : ValueObject
{
    public string FirstName { get; }
    public string LastName { get; }

    public string FullName => $"{FirstName} {LastName}";

    public PersonName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

        if (firstName.Length < 2)
            throw new ArgumentException("First name must be at least 2 characters.", nameof(firstName));

        if (lastName.Length < 2)
            throw new ArgumentException("Last name must be at least 2 characters.", nameof(lastName));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }

    public override string ToString() => FullName;
}
