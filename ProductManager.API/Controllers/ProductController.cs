using Microsoft.AspNetCore.Mvc;
using ProductManager.API.Dtos;
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

        [HttpGet]
        public async Task<ActionResult<PagedResult<ProductResponse>>> ObterProdutosAsync(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            Log.Information("Obtendo produtos - Página: {Page}, Tamanho: {PageSize}", page, pageSize);

            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var result = await _productService.ObterProdutosAsync(page, pageSize);

            var response = new PagedResult<ProductResponse>
            {
                Items = result.Items.Select(p => new ProductResponse
                {
                    Id = p.Id,
                    Nome = p.Nome ?? string.Empty,
                    Preco = p.Preco
                }),
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> ObterProdutoPorIdAsync(int id)
        {
            Log.Information("Obtendo produto por ID: {Id}", id);

            var product = await _productService.ObterProdutoPorIdAsync(id);

            var response = new ProductResponse
            {
                Id = product.Id,
                Nome = product.Nome ?? string.Empty,
                Preco = product.Preco
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponse>> CriarProdutoAsync(CreateProductRequest request)
        {
            Log.Information("Criando produto: {Nome}", request.Nome);

            var product = new Product
            {
                Nome = request.Nome,
                Preco = request.Preco
            };

            var created = await _productService.CriarProdutoAsync(product);

            var response = new ProductResponse
            {
                Id = created.Id,
                Nome = created.Nome ?? string.Empty,
                Preco = created.Preco
            };

            return Created($"/Product/{response.Id}", response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductResponse>> AtualizarProdutoPorIdAsync(int id, UpdateProductRequest request)
        {
            Log.Information("Atualizando produto por ID: {Id}", id);

            var product = new Product
            {
                Id = id,
                Nome = request.Nome,
                Preco = request.Preco
            };

            var updated = await _productService.AtualizarProdutoAsync(product);

            var response = new ProductResponse
            {
                Id = updated.Id,
                Nome = updated.Nome ?? string.Empty,
                Preco = updated.Preco
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletarProdutoPorId(int id)
        {
            Log.Information("Deletando produto por ID: {Id}", id);

            await _productService.DeletarProdutoPorIdAsync(id);

            return NoContent();
        }
    }
}
