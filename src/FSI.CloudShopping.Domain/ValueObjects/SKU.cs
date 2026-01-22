using FSI.CloudShopping.Domain.Core;
namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record SKU
    {
        public string Code { get; }
        public SKU(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new DomainException("SKU code cannot be empty.");
            Code = code.ToUpper().Trim();
        }
        public override string ToString() => Code;
    }
}