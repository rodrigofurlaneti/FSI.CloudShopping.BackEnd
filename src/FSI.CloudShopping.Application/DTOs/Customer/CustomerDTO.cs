namespace FSI.CloudShopping.Application.DTOs.Customer
{
    public record CustomerDTO
    {
        public int Id { get; init; }
        public string? Email { get; init; }
        public string CustomerType { get; init; }
        public Guid SessionToken { get; init; } 
        public bool IsActive { get; init; }
        public string? FullName { get; init; }
        public string? Document { get; init; }
        public string? CompanyName { get; init; } 
    }
}