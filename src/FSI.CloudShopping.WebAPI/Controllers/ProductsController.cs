using Microsoft.AspNetCore.Mvc;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Application.DTOs.Product;
namespace FSI.CloudShopping.API.Controllers
{
    public class ProductsController : BaseController<ProductDTO>
    {
        private readonly IProductAppService _productAppService;

        public ProductsController(IProductAppService productAppService) : base(productAppService)
        {
            _productAppService = productAppService;
        }
        [HttpGet("sku/{sku}")]
        public async Task<ActionResult<ProductDTO>> GetBySku(string sku)
        {
            var product = await _productAppService.GetBySkuAsync(sku);
            if (product == null) return NotFound();
            return Ok(product);
        }
        [HttpGet("category/{categoryId:int}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetByCategory(int categoryId)
        {
            var products = await _productAppService.GetByCategoryIdAsync(categoryId);
            return Ok(products);
        }
        [HttpPatch("{id:int}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] int quantity)
        {
            await _productAppService.UpdateStockAsync(id, quantity);
            return NoContent();
        }
    }
}