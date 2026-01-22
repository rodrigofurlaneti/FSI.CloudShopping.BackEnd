using FSI.CloudShopping.Domain.Core;
namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record ZipCode
    {
        public string Value { get; }
        public ZipCode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("ZipCode cannot be empty.");
            var cleanValue = new string(value.Where(char.IsDigit).ToArray());
            if (cleanValue.Length != 8)
                throw new DomainException("ZipCode must have 8 digits.");
            Value = cleanValue;
        }
        public string Formatted() => $"{Value.Substring(0, 5)}-{Value.Substring(5)}";
        public override string ToString() => Value;
    }
}