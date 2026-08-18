using Microsoft.EntityFrameworkCore;
using ProductManager.Core.Models;
using ProductManager.Infra.Interfaces;
using ProductManager.Infra.Models;
using Serilog;

namespace ProductManager.Infra.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;

        public ProductRepository(ProductContext context)
        {
            _context = context;
        }

        public async Task<Product> AtualizarProdutoAsync(Product product)
        {
            var existeProduto = await _context.Products.FindAsync(product.Id);

            if (existeProduto == null)
            {
                throw new KeyNotFoundException($"Produto com ID {product.Id} não encontrado.");
            }

            existeProduto.Nome = product.Nome;
            existeProduto.Preco = product.Preco;

            await _context.SaveChangesAsync();

            return existeProduto;
        }

        public async Task<Product> CriarProdutoAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task DeletarProdutoPorIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> ObterProdutoPorIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");
            }

            return product;
        }

        public async Task<PagedResult<Product>> ObterProdutosAsync(int page, int pageSize)
        {
            var totalCount = await _context.Products.CountAsync();

            var items = await _context.Products
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return PagedResult<Product>.Create(items, totalCount, page, pageSize);
        }

        public async Task<IEnumerable<Product>> ObterProdutosPorNomeAsync(string nome)
        {
            return await _context.Products
                .Where(p => p.Nome != null && p.Nome.Contains(nome))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> ObterProdutosPorPrecoAsync(decimal precoMin, decimal precoMax)
        {
            return await _context.Products
                .Where(p => p.Preco >= precoMin && p.Preco <= precoMax)
                .ToListAsync();
        }
    }
}
