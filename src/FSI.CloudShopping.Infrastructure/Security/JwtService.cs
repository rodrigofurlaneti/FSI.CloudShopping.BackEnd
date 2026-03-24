namespace FSI.CloudShopping.Infrastructure.Security;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Enums;
using Microsoft.Extensions.Options;

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; } = 60;
    public int RefreshTokenExpirationDays { get; set; } = 30;
}

public class JwtService : IJwtService
{
    private readonly JwtSettings _settings;

    public JwtService(IOptions<JwtSettings> options)
    {
        _settings = options.Value;
    }

    public string GenerateToken(Guid customerId, string email, CustomerType customerType)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, customerId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim("customerType", customerType.ToString()),
            new Claim("role", GetRoleFromCustomerType(customerType)),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        return Convert.ToBase64String(randomNumber);
    }

    public (Guid CustomerId, string Email, CustomerType CustomerType, string Role) ValidateToken(string token)
    {
        try
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
            var handler = new JwtSecurityTokenHandler();

            var principal = handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _settings.Issuer,
                ValidateAudience = true,
                ValidAudience = _settings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var customerId = Guid.Parse(principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? throw new InvalidOperationException("CustomerId not found"));
            var email = principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value ?? throw new InvalidOperationException("Email not found");
            var customerTypeStr = principal.FindFirst("customerType")?.Value ?? "Guest";
            var role = principal.FindFirst("role")?.Value ?? "Customer";

            var customerType = Enum.Parse<CustomerType>(customerTypeStr);

            return (customerId, email, customerType, role);
        }
        catch
        {
            throw new InvalidOperationException("Invalid token");
        }
    }

    public bool IsTokenValid(string token)
    {
        try
        {
            ValidateToken(token);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string GetRoleFromCustomerType(CustomerType customerType)
    {
        return customerType switch
        {
            CustomerType.Guest => "Guest",
            CustomerType.Lead => "Customer",
            CustomerType.B2C => "Customer",
            CustomerType.B2B => "Company",
            _ => "Guest"
        };
    }
}
