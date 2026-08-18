using Microsoft.AspNetCore.Mvc;
using ProductManager.Core.Models;
using ProductManager.Domain.Interfaces;
using Serilog;

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

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> ObterProdutoPorIdAsync(int id)
        {
            Log.Information("Obtendo produto por ID: {Id}", id);

            var product = await _productService.ObterProdutoPorIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> AtualizarProdutoPorIdAsync(int id, Product product)
        {
            Log.Information("Atualizando produto por ID: {Id}", id);

            if (id != product.Id)
            {
                return BadRequest();
            }

            await _productService.AtualizarProdutoAsync(product);

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CriarProdutoAsync(Product product)
        {
            var created = await _productService.CriarProdutoAsync(product);

            return CreatedAtAction(
                nameof(ObterProdutoPorIdAsync),
                new { id = created.Id },
                created);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletarProdutoPorId(int id)
        {
            Log.Information("Deletando produto por ID: {Id}", id);

            var product = await _productService.ObterProdutoPorIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            await _productService.DeletarProdutoPorIdAsync(id);

            return NoContent();
        }
    }
}
