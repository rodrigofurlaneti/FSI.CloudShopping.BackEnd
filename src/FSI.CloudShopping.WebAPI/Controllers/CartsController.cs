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
        [HttpGet("{token}")]
        public async Task<IActionResult> GetByToken(Guid token)
        {
            var cart = await _cartAppService.GetByTokenAsync(token);

            if (cart == null)
                return NotFound(new { message = "Carrinho não encontrado para este token." });

            return Ok(cart);
        }
        [HttpPost("add-item")]
        public async Task<IActionResult> AddItem([FromBody] AddCartItemDTO request)
        {
            var cart = await _cartAppService.AddItemAsync(request.SessionToken, request.ProductId, request.Quantity);
            return Ok(cart);
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