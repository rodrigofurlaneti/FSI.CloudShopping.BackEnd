using System;

namespace FSI.CloudShopping.Domain.Entities
{
    public class CustomerLocation
    {
        public int Id { get; set; }
        public int? IdCustomers { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Road { get; set; }
        public string Suburb { get; set; }
        public string CityDistrict { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Construtor opcional
        public CustomerLocation() { }

        public CustomerLocation(int? idCustomer, decimal latitude, decimal longitude,
                                string road = null, string suburb = null, string cityDistrict = null,
                                string city = null, string state = null, string country = null)
        {
            IdCustomers = idCustomer;
            Latitude = latitude;
            Longitude = longitude;
            Road = road;
            Suburb = suburb;
            CityDistrict = cityDistrict;
            City = city;
            State = state;
            Country = country;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
