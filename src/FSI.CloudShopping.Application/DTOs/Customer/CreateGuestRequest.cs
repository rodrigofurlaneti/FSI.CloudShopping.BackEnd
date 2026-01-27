namespace FSI.CloudShopping.Application.DTOs.Customer
{
    public record CreateGuestRequest
    {
        public decimal? Latitude { get; init; }
        public decimal? Longitude { get; init; }
        public DeviceInfoDto DeviceInfo { get; init; } = null!;
    }
}
