namespace FSI.CloudShopping.Application.DTOs
{
    public record AddressDTO(
        string Street,
        string Number,
        string Neighborhood,
        string City,
        string State,
        string ZipCode,
        bool IsDefault);
}
