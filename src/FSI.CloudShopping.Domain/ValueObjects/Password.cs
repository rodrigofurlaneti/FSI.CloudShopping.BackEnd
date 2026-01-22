namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record Password
    {
        public string Hash { get; }

        public Password(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException("Password hash is required.");

            Hash = hash;
        }
    }
}
