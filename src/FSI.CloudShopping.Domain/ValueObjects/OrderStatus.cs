namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record OrderStatus
    {
        public string Code { get; }
        private OrderStatus(string code) => Code = code;
        public static OrderStatus Pending => new("Pending");
        public static OrderStatus Paid => new("Paid");
        public static OrderStatus Shipped => new("Shipped");
        public static OrderStatus Delivered => new("Delivered");
        public static OrderStatus Cancelled => new("Cancelled");
        public static OrderStatus FromCode(string code)
        {
            return code?.Trim() switch
            {
                "Pending" => Pending,
                "Paid" => Paid,
                "Shipped" => Shipped,
                "Delivered" => Delivered,
                "Cancelled" => Cancelled,
                _ => throw new ArgumentException($"Status de pedido inválido: {code}")
            };
        }
        public override string ToString() => Code;
    }
}