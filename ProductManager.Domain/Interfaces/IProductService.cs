using ProductManager.Core.Models;

namespace ProductManager.Domain.Interfaces
{
    public interface IProductService
    {
        Task<PagedResult<Product>> ObterProdutosAsync(int page, int pageSize);
        Task<Product> ObterProdutoPorIdAsync(int id);
        Task<Product> CriarProdutoAsync(Product product);
        Task<Product> AtualizarProdutoAsync(Product product);
        Task DeletarProdutoPorIdAsync(int id);
    }
}
