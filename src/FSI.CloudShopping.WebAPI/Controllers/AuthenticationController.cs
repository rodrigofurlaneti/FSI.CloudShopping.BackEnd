using FSI.CloudShopping.Application.DTOs.Authentication; // Verifique se este caminho existe
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Application.Services;
using FSI.CloudShopping.Domain.Core;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace FSI.CloudShopping.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationAppService _authenticationAppService;

        public AuthenticationController(IAuthenticationAppService authenticationAppService)
        {
            _authenticationAppService = authenticationAppService;
        }
        [HttpPost("access")]
        public async Task<IActionResult> Access([FromBody] AuthenticationDTO request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var isAuthorized = await _authenticationAppService.GetAccessAsync(request.Email, request.Password);
            await _authenticationAppService.InsertAsync(request.Email, isAuthorized);
            if (!isAuthorized)
                return Unauthorized(new { message = "Credenciais inválidas." });
            return Ok(new
            {
                message = "Acesso autorizado.",
                token = Guid.NewGuid().ToString(),
                customerName = request.Email.Split('@')[0]
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] FSI.CloudShopping.Application.DTOs.Authentication.ResetPasswordRequest request)
        {
            try
            {
                await _authenticationAppService.ResetPasswordAsync(request.Email);
                return Ok(new { message = "Se o e-mail existir em nossa base, uma nova senha foi enviada." });
            }
            catch (DomainException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}