namespace FSI.CloudShopping.Application.DTOs.Customer
{
    public record RegisterLeadRequest(int CustomerId, string Email, string Password);
}
