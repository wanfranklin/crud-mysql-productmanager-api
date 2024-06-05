using Microsoft.EntityFrameworkCore;
using ProductManager.Core.Models;
using ProductManager.Infra.Interfaces;
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

                if (existeProduto != null)
                {
                    existeProduto.Nome = product.Nome;
                    existeProduto.Preco = product.Preco;

                    await _context.SaveChangesAsync();
                    
                    return existeProduto;
                }
                else
                {
                    throw new Exception("Produto não encontrado.");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao atualizar produto: {ex.Message}");
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
                Log.Error($"Erro ao criar produto: {ex.Message}");
                throw;
            }
        }

        public async Task DeletarProdutoPorIdAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Produto não encontrado!");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao deletar produto: {ex.Message}");
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
                    Log.Warning($"Produto com ID {id} não encontrado.");
                }
                return product;
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao obter produto por ID: {ex.Message}");
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
                Log.Error($"Erro ao obter produtos: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Product>> ObterProdutosPorNomeAsync(string nome)
        {
            try
            {
                return await _context.Products.Where(p => p.Nome.Contains(nome)).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao obter produtos por nome: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Product>> ObterProdutosPorPrecoAsync(decimal precoMin, decimal precoMax)
        {
            try
            {
                return await _context.Products.Where(p => p.Preco >= precoMin && p.Preco <= precoMax).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao obter produtos por preço: {ex.Message}");
                throw;
            }
        }
    }
}