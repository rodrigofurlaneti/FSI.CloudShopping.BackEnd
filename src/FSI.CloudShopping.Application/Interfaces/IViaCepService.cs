namespace FSI.CloudShopping.Application.Interfaces;

public interface IViaCepService
{
    Task<ViaCepAddressResult?> GetAddressByZipCodeAsync(string zipCode, CancellationToken cancellationToken = default);
}

public class ViaCepAddressResult
{
    public string Cep { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Complemento { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Localidade { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string Ibge { get; set; } = string.Empty;
    public string Gia { get; set; } = string.Empty;
    public string Ddd { get; set; } = string.Empty;
    public string Siafi { get; set; } = string.Empty;
}
