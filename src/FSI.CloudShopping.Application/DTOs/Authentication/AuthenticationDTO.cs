namespace FSI.CloudShopping.Application.DTOs.Authentication
{
    public record AuthenticationDTO(int Id,string Email, string Password, 
        bool isAuthenticationAccess, string CreatedAt);
}
