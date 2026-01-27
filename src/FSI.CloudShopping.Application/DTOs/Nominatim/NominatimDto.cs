using System.Text.Json.Serialization;

namespace FSI.CloudShopping.Application.DTOs.Location
{
    public class NominatimDto
    {
        [JsonPropertyName("lat")]
        public string? Latitude { get; set; }

        [JsonPropertyName("lon")]
        public string? Longitude { get; set; }

        [JsonPropertyName("display_name")]
        public string? DisplayName { get; set; }

        [JsonPropertyName("address")]
        public AddressDto? Address { get; set; }

        public class AddressDto
        {
            [JsonPropertyName("road")]
            public string? Road { get; set; }

            [JsonPropertyName("suburb")]
            public string? Suburb { get; set; }

            [JsonPropertyName("city_district")]
            public string? CityDistrict { get; set; }

            [JsonPropertyName("city")]
            public string? City { get; set; }

            [JsonPropertyName("state")]
            public string? State { get; set; }

            [JsonPropertyName("country")]
            public string? Country { get; set; }
        }
    }
}
