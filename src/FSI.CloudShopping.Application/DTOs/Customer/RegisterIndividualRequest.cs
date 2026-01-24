namespace FSI.CloudShopping.Application.DTOs.Customer
{
    public record RegisterIndividualRequest(int CustomerId, string Cpf, string FullName, DateTime? BirthDate);
}
