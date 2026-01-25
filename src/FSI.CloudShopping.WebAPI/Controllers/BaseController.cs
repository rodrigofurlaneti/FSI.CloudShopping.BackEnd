using Microsoft.AspNetCore.Mvc;
using FSI.CloudShopping.Application.Interfaces;

namespace FSI.CloudShopping.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<TDTO> : ControllerBase where TDTO : class
    {
        protected readonly IBaseAppService<TDTO> _appService;

        protected BaseController(IBaseAppService<TDTO> appService)
        {
            _appService = appService;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TDTO>>> Get()
        {
            var result = await _appService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public virtual async Task<ActionResult<TDTO>> Get(int id)
        {
            var result = await _appService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult<TDTO>> Post([FromBody] TDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _appService.AddAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = 0 }, result); // Ajuste o 'id' conforme sua DTO
        }

        [HttpPut]
        public virtual async Task<IActionResult> Put([FromBody] TDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _appService.UpdateAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            await _appService.RemoveAsync(id);
            return NoContent();
        }
    }
}