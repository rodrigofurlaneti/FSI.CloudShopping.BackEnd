namespace FSI.CloudShopping.Application.Interfaces;

using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Enums;

public interface IJwtService
{
    string GenerateToken(Guid customerId, string email, CustomerType customerType);
    string GenerateRefreshToken();
    (Guid CustomerId, string Email, CustomerType CustomerType, string Role) ValidateToken(string token);
    bool IsTokenValid(string token);
}
