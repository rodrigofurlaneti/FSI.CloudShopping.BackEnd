namespace FSI.CloudShopping.Application.DTOs;

public record LoginRequest(string Email, string Password);

public record LoginResponse(Guid CustomerId, string Email, string Token, string RefreshToken, DateTime ExpiresIn);

public record RegisterGuestRequest(string Email);

public record RegisterIndividualRequest(string Email, string Password, string FirstName, string LastName, string TaxId, DateTime BirthDate);

public record RegisterCompanyRequest(string Email, string Password, string CompanyName, string BusinessTaxId, string? StateTaxId = null, string? TradeName = null);

public record RefreshTokenRequest(string RefreshToken);

public record RefreshTokenResponse(string Token, string RefreshToken, DateTime ExpiresIn);

public record RevokeTokenRequest(string Token);

public record ForgotPasswordRequest(string Email);

public record ResetPasswordRequest(string Token, string NewPassword);
