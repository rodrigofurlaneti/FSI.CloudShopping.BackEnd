namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record AddressType
    {
        public string Description { get; }
        private AddressType(string description) => Description = description;
        public static AddressType Shipping => new("Shipping");
        public static AddressType Billing => new("Billing");
        public static AddressType Commercial => new("Commercial");
        public static AddressType FromString(string value)
        {
            return value switch
            {
                "Shipping" => Shipping,
                "Billing" => Billing,
                "Commercial" => Commercial,
                _ => throw new ArgumentException("Invalid Address Type")
            };
        }
    }
}
