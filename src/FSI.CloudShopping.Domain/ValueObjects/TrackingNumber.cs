namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record TrackingNumber
    {
        public string Code { get; }
        public TrackingNumber(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Tracking number cannot be empty.");
            Code = code.ToUpper().Trim();
        }
    }
}
