namespace FSI.CloudShopping.Application.DTOs.Customer
{
    public record CreateGuestResponse
    {
        public Guid SessionToken { get; init; }
        public DateTime ExpiresAt { get; init; }
    }
}
