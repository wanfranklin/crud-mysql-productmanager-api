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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Nome).IsRequired().HasMaxLength(255);
                entity.Property(p => p.Preco).IsRequired().HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Nome = "Notebook Dell Inspiron 15", Preco = 4599.90m },
                new Product { Id = 2, Nome = "Mouse Logitech MX Master 3S", Preco = 349.90m },
                new Product { Id = 3, Nome = "Teclado Mecânico Keychron K2", Preco = 499.90m },
                new Product { Id = 4, Nome = "Monitor LG UltraWide 29\"", Preco = 1899.90m },
                new Product { Id = 5, Nome = "Webcam Logitech C920", Preco = 299.90m },
                new Product { Id = 6, Nome = "Fone de Ouvido JBL Tune 510BT", Preco = 199.90m },
                new Product { Id = 7, Nome = "Capa para iPhone 15 Pro", Preco = 89.90m },
                new Product { Id = 8, Nome = "Carregador USB-C 65W Anker", Preco = 159.90m },
                new Product { Id = 9, Nome = "Caderno Tilibra 10 Matérias", Preco = 29.90m },
                new Product { Id = 10, Nome = "Caneta Bic Cristal (Caixa c/ 20)", Preco = 18.90m },
                new Product { Id = 11, Nome = "Lápis de Cor Faber-Castell (12 cores)", Preco = 34.90m },
                new Product { Id = 12, Nome = "Café Pilão Torrado e Moído 500g", Preco = 16.90m },
                new Product { Id = 13, Nome = "Azeite Extra Virgem Gallo 500ml", Preco = 39.90m },
                new Product { Id = 14, Nome = "Chocolate Garoto Ao Leite 90g", Preco = 8.90m },
                new Product { Id = 15, Nome = "Refrigerante Coca-Cola 2L", Preco = 9.90m },
                new Product { Id = 16, Nome = "Lâmpada LED Bulb Philco 9W", Preco = 12.90m },
                new Product { Id = 17, Nome = "Extensão 6 Tomadas Prolam 3m", Preco = 29.90m },
                new Product { Id = 18, Nome = "Filtro de Água Brita Classic", Preco = 89.90m },
                new Product { Id = 19, Nome = "Bola de Futebol Penalty Society", Preco = 79.90m },
                new Product { Id = 20, Nome = "Garrafa Térmica Lily 500ml", Preco = 69.90m }
            );
        }
    }
}
