using Microsoft.EntityFrameworkCore;
using ProductManager.Core.Models;

namespace ProductManager.Infra.Models
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }
    }
}
