using FSI.CloudShopping.Domain.Core;
namespace FSI.CloudShopping.Domain.ValueObjects
{
    public class GeoLocation
    {
        public decimal Latitude { get; }
        public decimal Longitude { get; }

        public GeoLocation(decimal latitude, decimal longitude)
        {
            if (latitude < -90 || latitude > 90)
                throw new DomainException("Latitude inválida.");

            if (longitude < -180 || longitude > 180)
                throw new DomainException("Longitude inválida.");

            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
