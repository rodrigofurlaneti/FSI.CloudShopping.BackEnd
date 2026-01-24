namespace FSI.CloudShopping.Application.DTOs.Customer
{
    public record RegisterCompanyRequest(int CustomerId, string Cnpj, string CompanyName, string? StateTaxId);
}
