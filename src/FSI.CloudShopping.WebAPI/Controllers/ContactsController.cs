using Microsoft.AspNetCore.Mvc;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Application.DTOs.Contact;
namespace FSI.CloudShopping.API.Controllers
{
    public class ContactsController : BaseController<ContactDTO>
    {
        private readonly IContactAppService _contactAppService;

        public ContactsController(IContactAppService contactAppService) : base(contactAppService)
        {
            _contactAppService = contactAppService;
        }
        [HttpGet("customer/{customerId:int}")]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> GetByCustomer(int customerId)
        {
            var contacts = await _contactAppService.GetByCustomerIdAsync(customerId);
            return Ok(contacts);
        }
    }
}