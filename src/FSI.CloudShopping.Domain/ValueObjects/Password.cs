using FSI.CloudShopping.Domain.Core;
namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record Password
    {
        public string Hash { get; }
        public Password(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new DomainException("Password hash is required.");
            if (hash.Length < 20)
                throw new DomainException("Invalid password hash format.");

            Hash = hash;
        }
        public override string ToString() => "********"; 
    }
}
