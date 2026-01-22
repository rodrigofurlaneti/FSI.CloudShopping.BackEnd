using FSI.CloudShopping.Domain.Core;
namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record PersonName
    {
        public string FullName { get; }
        public PersonName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new DomainException("Name cannot be empty.");
            if (!fullName.Trim().Contains(" "))
                throw new DomainException("Please provide a full name (first and last name).");
            FullName = fullName.Trim();
        }
        public static implicit operator string(PersonName name) => name.FullName;
        public override string ToString() => FullName;
    }
}