namespace FSI.CloudShopping.Infrastructure.Services;

using System.Net.Http.Json;
using FSI.CloudShopping.Application.Interfaces;
using Microsoft.Extensions.Logging;

public class ViaCepService : IViaCepService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ViaCepService> _logger;
    private const string BaseUrl = "https://viacep.com.br/ws";

    public ViaCepService(HttpClient httpClient, ILogger<ViaCepService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ViaCepAddressResult?> GetAddressByZipCodeAsync(string zipCode, CancellationToken cancellationToken = default)
    {
        try
        {
            var cleanZipCode = zipCode.Replace("-", "").Replace(".", "");
            var url = $"{BaseUrl}/{cleanZipCode}/json";

            var response = await _httpClient.GetAsync(url, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to fetch address for zipcode {ZipCode}", zipCode);
                return null;
            }

            var result = await response.Content.ReadAsAsync<ViaCepAddressResult>(cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching address from ViaCEP for zipcode {ZipCode}", zipCode);
            return null;
        }
    }
}
