using Microsoft.AspNetCore.Mvc;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Application.DTOs.Customer;
namespace FSI.CloudShopping.API.Controllers
{
    public class CustomersController : BaseController<CustomerDTO>
    {
        private readonly ICustomerAppService _customerAppService;

        public CustomersController(ICustomerAppService customerAppService) : base(customerAppService)
        {
            _customerAppService = customerAppService;
        }
        [HttpPost("guest")]
        public async Task<ActionResult<CreateGuestResponse>> CreateGuest(
            [FromBody] CreateGuestRequest request)
        {
            var response = await _customerAppService.CreateGuestAsync(request);
            Response.Cookies.Append(
                "session_token",
                response.SessionToken.ToString(),
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = response.ExpiresAt
                });
            return Ok(response);
        }
        [HttpPost("register-lead")]
        public async Task<IActionResult> RegisterLead([FromBody] RegisterLeadRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _customerAppService.RegisterLeadAsync(request);
            return Ok(new { message = "Lead registrado com sucesso." });
        }
        [HttpGet("email/{email}")]
        public async Task<ActionResult<CustomerDTO>> GetByEmail(string email)
        {
            var customer = await _customerAppService.GetByEmailAsync(email);
            if (customer == null) return NotFound();
            return Ok(customer);
        }
        [HttpPut("convert-to-individual")]
        public async Task<IActionResult> UpdateToIndividual([FromBody] RegisterIndividualRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _customerAppService.UpdateToIndividualAsync(request);
            return NoContent();
        }
        [HttpPut("convert-to-company")]
        public async Task<IActionResult> UpdateToCompany([FromBody] RegisterCompanyRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _customerAppService.UpdateToCompanyAsync(request);
            return NoContent();
        }
    }
}