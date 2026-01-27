namespace FSI.CloudShopping.Application.DTOs.Customer
{
    public record DeviceInfoDto
    {
        public string? UserAgent { get; init; }
        public string? Platform { get; init; }
        public string? Language { get; init; }
        public string? TimeZone { get; init; }
    }
}
