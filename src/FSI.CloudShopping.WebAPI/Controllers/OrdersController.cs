using Microsoft.AspNetCore.Mvc;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Application.DTOs.Order;
namespace FSI.CloudShopping.API.Controllers
{
    public class OrdersController : BaseController<OrderDTO>
    {
        private readonly IOrderAppService _orderAppService;
        public OrdersController(IOrderAppService orderAppService) : base(orderAppService)
        {
            _orderAppService = orderAppService;
        }
        [HttpPost("checkout")]
        public async Task<ActionResult<OrderDTO>> Checkout([FromBody] CheckoutRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = await _orderAppService.PlaceOrderAsync(request);
            return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        }
        [HttpGet("history/customer/{customerId:int}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetHistory(int customerId)
        {
            var history = await _orderAppService.GetCustomerHistoryAsync(customerId);
            return Ok(history);
        }
    }
}