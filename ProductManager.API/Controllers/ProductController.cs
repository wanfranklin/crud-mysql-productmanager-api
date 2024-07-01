using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.Sig;
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

        [HttpGet("(id)")]
        public async Task<ActionResult<Product>> ObterProdutoPorIdAsync(int id)
        {
            Log.Information("Obtendo produto por ID:", id);

            var product = await _productService.ObterProdutoPorIdAsync(id);

            if (product == null)
            {
                return BadRequest();
            }

            return Ok(product);
        }

        [HttpPut("(id)")]
        public async Task<ActionResult> AtualizarProdutoPorIdAsync(int id, Product product)
        {
            Log.Information("Atualizando produto por ID:", id);

            if (id != product.Id)
            {
                return BadRequest();
            }

            await _productService.AtualizarProdutoAsync(product);

            Console.WriteLine("Dado atualizado com sucesso.");

            return Content("Dado atualizado com sucesso.");
        }

        [HttpPost]
        public async Task<ActionResult> CriarProdutoAsync(Product product)
        {
            await _productService.CriarProdutoAsync(product);
            return Content("Sucesso.");
        }

        [HttpDelete("(id)")]
        public async Task<ActionResult> DeletarProdutoPorId(int id)
        {
            Log.Information("Deletando produto por ID:", id);
            await _productService.DeletarProdutoPorIdAsync(id);
            return Content("Sucesso.");
        }
    }
}