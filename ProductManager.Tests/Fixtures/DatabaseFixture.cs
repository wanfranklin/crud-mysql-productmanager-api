using Microsoft.EntityFrameworkCore;
using ProductManager.Infra.Models;

namespace ProductManager.Tests.Fixtures
{
    public class DatabaseFixture : IDisposable
    {
        public ProductContext Context { get; private set; }

        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<ProductContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            Context = new ProductContext(options);
            Context.Database.EnsureCreated();
            SeedData();
        }

        private void SeedData()
        {
            if (!Context.Products.Any())
            {
                Context.Products.AddRange(
                    new Core.Models.Product { Id = 1, Nome = "Notebook Dell Inspiron 15", Preco = 4599.90m },
                    new Core.Models.Product { Id = 2, Nome = "Mouse Logitech MX Master 3S", Preco = 349.90m },
                    new Core.Models.Product { Id = 3, Nome = "Teclado Mecânico Keychron K2", Preco = 499.90m },
                    new Core.Models.Product { Id = 4, Nome = "Monitor LG UltraWide 29\"", Preco = 1899.90m },
                    new Core.Models.Product { Id = 5, Nome = "Webcam Logitech C920", Preco = 299.90m }
                );
                Context.SaveChanges();
            }
        }

        public void ResetDatabase()
        {
            Context.Products.RemoveRange(Context.Products);
            Context.SaveChanges();
            SeedData();
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}
