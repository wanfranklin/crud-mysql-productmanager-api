using ProductManager.Core.Models;

namespace ProductManager.Infra.Interfaces
{
    public interface IProductRepository
    {
        Task<PagedResult<Product>> ObterProdutosAsync(int page, int pageSize);
        Task<Product> ObterProdutoPorIdAsync(int id);
        Task<Product> CriarProdutoAsync(Product product);
        Task<Product> AtualizarProdutoAsync(Product product);
        Task DeletarProdutoPorIdAsync(int id);
        Task<IEnumerable<Product>> ObterProdutosPorNomeAsync(string nome);
        Task<IEnumerable<Product>> ObterProdutosPorPrecoAsync(decimal precoMin, decimal precoMax);
    }
}
