using FSI.CloudShopping.Application.DTOs.Location;
using FSI.CloudShopping.Domain.Entities;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Globalization; 
namespace FSI.CloudShopping.Application.Brokers
{
    public class NominatimService : INominatimService
    {
        private readonly HttpClient _httpClient;

        public NominatimService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/116.0.0.0 Safari/537.36"
            );
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
            _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
            _httpClient.DefaultRequestHeaders.From = "rodrigofurlaneti31@hotmail.com";
        }

        public async Task<CustomerLocation> GetLocationAsync(decimal latitude, decimal longitude)
        {
            try
            {
                string url = string.Create(CultureInfo.InvariantCulture, $"https://nominatim.openstreetmap.org/reverse?lat={latitude}&lon={longitude}&format=json"); 
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode(); 
                var json = await response.Content.ReadAsStringAsync();
                var nominatimDto = System.Text.Json.JsonSerializer.Deserialize<NominatimDto>(json);
                var location = new CustomerLocation
                {
                    Latitude = latitude,
                    Longitude = longitude,
                    Road = nominatimDto?.Address?.Road,
                    Suburb = nominatimDto?.Address?.Suburb,
                    CityDistrict = nominatimDto?.Address?.CityDistrict,
                    City = nominatimDto?.Address?.City,
                    State = nominatimDto?.Address?.State,
                    Country = nominatimDto?.Address?.Country
                };

                return location;
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"Erro ao chamar Nominatim: {httpEx.Message}");
            }
            catch (System.Text.Json.JsonException jsonEx)
            {
                Console.WriteLine($"Erro ao desserializar JSON do Nominatim: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
            }

            return new CustomerLocation
            {
                Latitude = latitude,
                Longitude = longitude
            };
        }
    }
}
