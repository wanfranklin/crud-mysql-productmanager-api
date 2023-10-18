using ProductManager.Core.Models;

namespace ProductManager.Infra.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> ObterProdutosAsync();
        Task<Product> ObterProdutoPorIdAsync(int id);
        Task<Product> CriarProdutoAsync(Product product);
        Task<Product> AtualizarProdutoAsync(Product product);
        Task DeletarProdutosAsync(int id);
    }
}
