using Microsoft.AspNetCore.Mvc;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Application.DTOs.Cart;
namespace FSI.CloudShopping.API.Controllers
{
    public class CartsController : BaseController<CartDTO>
    {
        private readonly ICartAppService _cartAppService;
        public CartsController(ICartAppService cartAppService) : base(cartAppService)
        {
            _cartAppService = cartAppService;
        }
        [HttpPost("merge")]
        public async Task<IActionResult> Merge([FromQuery] Guid visitorToken, [FromQuery] int customerId)
        {
            await _cartAppService.MergeAfterLoginAsync(visitorToken, customerId);
            return Ok(new { message = "Carrinhos mesclados com sucesso." });
        }
        [HttpDelete("clear/{token}")]
        public async Task<IActionResult> Clear(string token)
        {
            await _cartAppService.ClearCartAsync(token);
            return NoContent();
        }
    }
}