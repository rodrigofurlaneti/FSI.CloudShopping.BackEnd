using FSI.CloudShopping.Domain.Core;
namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record Email
    {
        public string Address { get; }
        public Email(string address)
        {
            if (string.IsNullOrWhiteSpace(address) || !address.Contains("@"))
                throw new DomainException("Invalid email address format.");

            Address = address;
        }
        public static implicit operator string(Email email) => email.Address;
    }
}