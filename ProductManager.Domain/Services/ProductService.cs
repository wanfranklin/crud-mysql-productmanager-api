using ProductManager.Core.Models;
using ProductManager.Domain.Interfaces;
using ProductManager.Infra.Interfaces;

namespace ProductManager.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository) 
        {
            _productRepository = productRepository;
        }

        public async Task<Product> AtualizarProdutoAsync(Product product)
        {
            return await _productRepository.AtualizarProdutoAsync(product);
        }

        public async Task<Product> CriarProdutoAsync(Product product)
        {
            return await _productRepository.CriarProdutoAsync(product);
        }

        public async Task DeletarProdutosAsync(int id)
        {
            await _productRepository.DeletarProdutosAsync(id);
        }

        public async Task<Product> ObterProdutoPorIdAsync(int id)
        {
            return await _productRepository.ObterProdutoPorIdAsync(id);
        }

        public async Task<IEnumerable<Product>> ObterProdutosAsync()
        {
            return await _productRepository.ObterProdutosAsync();
        }
    }
}