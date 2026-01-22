using FSI.CloudShopping.Domain.Core;
namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record TrackingNumber
    {
        public string Code { get; }
        public TrackingNumber(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new DomainException("Tracking number cannot be empty.");
            Code = code.ToUpper().Trim();
        }
        public override string ToString() => Code;
    }
}