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
    }
}
