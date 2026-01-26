using System.Text.RegularExpressions;
using FSI.CloudShopping.Domain.Core;

namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record Email
    {
        public string Address { get; }

        private static readonly Regex EmailRegex = new Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public Email(string address)
        {
            if (string.IsNullOrWhiteSpace(address) || !EmailRegex.IsMatch(address))
                throw new DomainException("Invalid email address format.");

            Address = address.ToLower().Trim();
        }

        public static implicit operator string(Email email) => email.Address;
        public override string ToString() => Address;
    }
}