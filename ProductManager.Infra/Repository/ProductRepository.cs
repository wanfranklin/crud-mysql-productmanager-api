using ProductManager.Core.Models;
using ProductManager.Infra.Interfaces;

namespace ProductManager.Infra.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products;

        public ProductRepository()
        {
            _products = new List<Product>()
            {
                new Product { Id = 1, Nome = "Produto 1", Preco = 10 },
                new Product { Id = 2, Nome = "Produto 2", Preco = 20 },
                new Product { Id = 3, Nome = "Produto 3", Preco = 20 },
            };
        }

        public async Task<Product> AtualizarProdutoAsync(Product product)
        {
            Product existeProduto = _products.FirstOrDefault(product => product.Id == product.Id);

            if (existeProduto != null)
            {
                existeProduto.Nome = product.Nome;
                existeProduto.Preco = product.Preco;
                return existeProduto;
            }
            else
            {
                throw new Exception("Produto não encontrado.");
            }
        }

        public async Task<Product> CriarProdutoAsync(Product product)
        {
            int novoProduto = _products.Max(p => p.Id) + 1;

            product.Id = novoProduto;

            _products.Add(product);

            return product;
        }

        public async Task DeletarProdutosAsync(int id)
        {
            Product product = _products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                _products.Remove(product);
            }
            else
            {
                throw new Exception("Produto não encontrado!");
            }
        }

        public async Task<Product> ObterProdutoPorIdAsync(int id)
        {
            return await Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
        }

        public async Task<IEnumerable<Product>> ObterProdutosAsync()
        {
            return await Task.FromResult(_products);
        }
    }
}
