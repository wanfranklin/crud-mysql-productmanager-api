using ProductManager.Core.Models;

namespace ProductManager.Infra.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> ObterProdutosAsync();
        Task<Product> ObterProdutoPorIdAsync(int id);
        Task<Product> CriarProdutoAsync(Product product);
        Task<Product> AtualizarProdutoAsync(Product product);
        Task DeletarProdutoPorIdAsync(int id);
        Task<IEnumerable<Product>> ObterProdutosPorNomeAsync(string nome);
        Task<IEnumerable<Product>> ObterProdutosPorPrecoAsync(decimal precoMin, decimal precoMax);
    }
}
