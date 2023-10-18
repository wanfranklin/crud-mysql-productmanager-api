using Microsoft.AspNetCore.Mvc;
using ProductManager.Core.Models;
using ProductManager.Core.Uteis;
using ProductManager.Domain.Interfaces;

namespace ProductManager.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("(id)")]
        public async Task<ActionResult<Product>> ObterProdutoPorIdAsync(int id)
        {
            Log.Registrar("Obtendo produto por ID:", id);

            var product = await _productService.ObterProdutoPorIdAsync(id);

            if (product == null)
            {
                return BadRequest();
            }

            return Ok(product);
        }
    }
}