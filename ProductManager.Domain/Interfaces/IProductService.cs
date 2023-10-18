using ProductManager.Core.Models;

namespace ProductManager.Domain.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> ObterProdutosAsync();
        Task<Product> ObterProdutoPorIdAsync(int id);
        Task<Product> CriarProdutoAsync(Product product);
        Task<Product> AtualizarProdutoAsync(Product product);
        Task DeletarProdutosAsync(int id);
    }
}