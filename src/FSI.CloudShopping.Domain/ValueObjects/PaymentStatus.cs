namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record PaymentStatus
    {
        public string Description { get; }
        private PaymentStatus(string description) => Description = description;

        public static PaymentStatus Pending => new("Pending");
        public static PaymentStatus Captured => new("Captured");
        public static PaymentStatus Refunded => new("Refunded");
        public static PaymentStatus Failed => new("Failed");
        public static PaymentStatus FromString(string value)
        {
            return value?.Trim() switch
            {
                "Pending" => Pending,
                "Captured" => Captured,
                "Refunded" => Refunded,
                "Failed" => Failed,
                _ => throw new ArgumentException($"Status de pagamento inválido: {value}")
            };
        }
        public override string ToString() => Description;
    }
}