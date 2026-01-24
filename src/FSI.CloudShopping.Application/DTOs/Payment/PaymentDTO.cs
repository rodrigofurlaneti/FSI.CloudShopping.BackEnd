namespace FSI.CloudShopping.Application.DTOs
{
    public record PaymentDTO
    {
        public int Id { get; init; }
        public int OrderId { get; init; }
        public decimal Amount { get; init; }
        public string PaymentMethod { get; init; } 
        public string Status { get; init; }
        public string TransactionId { get; init; }
        public DateTime PaymentDate { get; init; }
    }
}