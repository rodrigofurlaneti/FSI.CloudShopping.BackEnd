namespace FSI.CloudShopping.WebAPI.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FSI.CloudShopping.Application.Commands.Auth;
using FSI.CloudShopping.Application.Commands.Customer;
using FSI.CloudShopping.Application.DTOs;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : BaseApiController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>POST /api/v1/auth/login — Autentica e retorna JWT + Refresh Token.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new LoginCommand(request.Email, request.Password), ct);
        return HandleResult(result);
    }

    /// <summary>POST /api/v1/auth/guest — Cria sessão de visitante (Guest).</summary>
    [HttpPost("guest")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterGuest([FromBody] RegisterGuestRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new RegisterGuestCommand(request.Email), ct);
        return HandleResult(result);
    }

    /// <summary>POST /api/v1/auth/register/individual — Cadastro completo B2C (Pessoa Física).</summary>
    [HttpPost("register/individual")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterIndividual([FromBody] RegisterIndividualRequest request, CancellationToken ct)
    {
        // Step 1: Become Lead (email + password)
        var leadResult = await _mediator.Send(
            new RegisterGuestCommand(request.Email), ct);

        if (!leadResult.IsSuccess)
            return HandleResult(leadResult);

        // Step 2: Register as B2C Individual
        var leadCustomer = ((dynamic)leadResult).Value;
        var individualResult = await _mediator.Send(
            new RegisterIndividualCommand(
                (Guid)leadCustomer.Id,
                request.TaxId,
                request.FullName,
                request.BirthDate), ct);

        return HandleResult(individualResult);
    }

    /// <summary>POST /api/v1/auth/register/company — Cadastro completo B2B (Pessoa Jurídica).</summary>
    [HttpPost("register/company")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterCompany([FromBody] RegisterCompanyRequest request, CancellationToken ct)
    {
        var guestResult = await _mediator.Send(new RegisterGuestCommand(request.Email), ct);
        if (!guestResult.IsSuccess) return HandleResult(guestResult);

        var guestCustomer = ((dynamic)guestResult).Value;
        var companyResult = await _mediator.Send(
            new RegisterCompanyCommand(
                (Guid)guestCustomer.Id,
                request.BusinessTaxId,
                request.CompanyName,
                request.TradeName,
                request.StateTaxId), ct);

        return HandleResult(companyResult);
    }

    /// <summary>POST /api/v1/auth/refresh — Renova Access Token via Refresh Token.</summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new RefreshTokenCommand(request.RefreshToken), ct);
        return HandleResult(result);
    }

    /// <summary>POST /api/v1/auth/logout — Revoga Refresh Token do usuário autenticado.</summary>
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        var customerId = GetCurrentCustomerId();
        var result = await _mediator.Send(new RevokeTokenCommand(customerId), ct);
        return HandleResult(result);
    }

    /// <summary>POST /api/v1/auth/forgot-password — Envia e-mail de redefinição de senha.</summary>
    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new ForgotPasswordCommand(request.Email), ct);
        return HandleResult(result);
    }

    /// <summary>POST /api/v1/auth/reset-password — Redefine senha com token de redefinição.</summary>
    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new ResetPasswordCommand(request.Token, request.NewPassword), ct);
        return HandleResult(result);
    }
}
