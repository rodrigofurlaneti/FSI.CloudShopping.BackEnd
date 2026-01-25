using Microsoft.AspNetCore.Mvc;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Application.DTOs.Address;
namespace FSI.CloudShopping.API.Controllers
{
    public class AddressesController : BaseController<AddressDTO>
    {
        private readonly IAddressAppService _addressAppService;

        public AddressesController(IAddressAppService addressAppService) : base(addressAppService)
        {
            _addressAppService = addressAppService;
        }
        [HttpGet("customer/{customerId:int}")]
        public async Task<ActionResult<IEnumerable<AddressDTO>>> GetByCustomer(int customerId)
        {
            var addresses = await _addressAppService.GetByCustomerIdAsync(customerId);
            return Ok(addresses);
        }
        [HttpPatch("{id:int}/set-default/customer/{customerId:int}")]
        public async Task<IActionResult> SetDefault(int id, int customerId)
        {
            await _addressAppService.SetDefaultAddressAsync(id, customerId);
            return NoContent();
        }
    }
}