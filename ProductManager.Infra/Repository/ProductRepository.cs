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
            try
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
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao atualizar produto");
                throw;
            }
        }

        public async Task<Product> CriarProdutoAsync(Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return product;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao criar produto");
                throw;
            }
        }

        public async Task DeletarProdutoPorIdAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao deletar produto");
                throw;
            }
        }

        public async Task<Product> ObterProdutoPorIdAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    Log.Warning("Produto com ID {Id} não encontrado.", id);
                }

                return product!;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao obter produto por ID");
                throw;
            }
        }

        public async Task<IEnumerable<Product>> ObterProdutosAsync()
        {
            try
            {
                return await _context.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao obter produtos");
                throw;
            }
        }

        public async Task<IEnumerable<Product>> ObterProdutosPorNomeAsync(string nome)
        {
            try
            {
                return await _context.Products
                    .Where(p => p.Nome != null && p.Nome.Contains(nome))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao obter produtos por nome");
                throw;
            }
        }

        public async Task<IEnumerable<Product>> ObterProdutosPorPrecoAsync(decimal precoMin, decimal precoMax)
        {
            try
            {
                return await _context.Products
                    .Where(p => p.Preco >= precoMin && p.Preco <= precoMax)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao obter produtos por preço");
                throw;
            }
        }
    }
}
