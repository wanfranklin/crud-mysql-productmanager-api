using FluentAssertions;
using ProductManager.Core.Models;

namespace ProductManager.Tests.Unit.Core
{
    public class PagedResultTests
    {
        [Fact]
        public void Create_DeveCalcularTotalPages_Corretamente()
        {
            var items = Enumerable.Range(1, 20).Select(i => new Product { Id = i, Nome = $"Produto {i}", Preco = i * 10m });

            var result = PagedResult<Product>.Create(items, 20, 1, 10);

            result.TotalPages.Should().Be(2);
        }

        [Fact]
        public void Create_DeveRetornarItemsFornecidos()
        {
            var items = new List<Product>
            {
                new Product { Id = 1, Nome = "A", Preco = 10m },
                new Product { Id = 2, Nome = "B", Preco = 20m }
            };

            var result = PagedResult<Product>.Create(items, 2, 1, 10);

            result.Items.Should().HaveCount(2);
            result.Items.Should().Contain(p => p.Nome == "A");
            result.Items.Should().Contain(p => p.Nome == "B");
        }

        [Fact]
        public void Create_ComTotalZero_DeveRetornarZeroPages()
        {
            var items = Enumerable.Empty<Product>();

            var result = PagedResult<Product>.Create(items, 0, 1, 10);

            result.TotalPages.Should().Be(0);
            result.Items.Should().BeEmpty();
        }

        [Fact]
        public void Create_DevePreservarCurrentPageEEPageSize()
        {
            var items = Enumerable.Range(1, 5).Select(i => new Product { Id = i, Nome = $"P{i}", Preco = i * 10m });

            var result = PagedResult<Product>.Create(items, 15, 3, 5);

            result.CurrentPage.Should().Be(3);
            result.PageSize.Should().Be(5);
        }

        [Fact]
        public void Create_ComUmItem_DeveRetornarUmItem()
        {
            var items = new List<Product>
            {
                new Product { Id = 1, Nome = "Único", Preco = 99.90m }
            };

            var result = PagedResult<Product>.Create(items, 1, 1, 10);

            result.Items.Should().HaveCount(1);
            result.TotalCount.Should().Be(1);
            result.TotalPages.Should().Be(1);
        }
    }
}
