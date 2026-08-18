using FluentAssertions;
using Moq;
using ProductManager.Core.Models;
using ProductManager.Domain.Services;
using ProductManager.Infra.Interfaces;

namespace ProductManager.Tests.Unit.Domain
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _service = new ProductService(_repositoryMock.Object);
        }

        [Fact]
        public async Task ObterProdutos_DeveChamarRepository_ComParametrosCorretos()
        {
            var pagedResult = PagedResult<Product>.Create(
                new List<Product> { new Product { Id = 1, Nome = "Teste", Preco = 10m } },
                1, 1, 10);

            _repositoryMock
                .Setup(r => r.ObterProdutosAsync(1, 10))
                .ReturnsAsync(pagedResult);

            var result = await _service.ObterProdutosAsync(1, 10);

            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            _repositoryMock.Verify(r => r.ObterProdutosAsync(1, 10), Times.Once);
        }

        [Fact]
        public async Task ObterProdutoPorId_DeveRetornarProduto()
        {
            var product = new Product { Id = 1, Nome = "Notebook", Preco = 4599.90m };

            _repositoryMock
                .Setup(r => r.ObterProdutoPorIdAsync(1))
                .ReturnsAsync(product);

            var result = await _service.ObterProdutoPorIdAsync(1);

            result.Should().NotBeNull();
            result.Nome.Should().Be("Notebook");
            result.Preco.Should().Be(4599.90m);
        }

        [Fact]
        public async Task ObterProdutoPorId_NaoEncontrado_DeveLancarKeyNotFoundException()
        {
            _repositoryMock
                .Setup(r => r.ObterProdutoPorIdAsync(999))
                .ThrowsAsync(new KeyNotFoundException("Produto com ID 999 não encontrado."));

            var act = () => _service.ObterProdutoPorIdAsync(999);

            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*999*");
        }

        [Fact]
        public async Task CriarProduto_DeveChamarRepositoryERetornarProduto()
        {
            var newProduct = new Product { Nome = "Fone JBL", Preco = 199.90m };
            var createdProduct = new Product { Id = 6, Nome = "Fone JBL", Preco = 199.90m };

            _repositoryMock
                .Setup(r => r.CriarProdutoAsync(It.IsAny<Product>()))
                .ReturnsAsync(createdProduct);

            var result = await _service.CriarProdutoAsync(newProduct);

            result.Should().NotBeNull();
            result.Id.Should().Be(6);
            result.Nome.Should().Be("Fone JBL");
            _repositoryMock.Verify(r => r.CriarProdutoAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task AtualizarProduto_DeveChamarRepository()
        {
            var product = new Product { Id = 1, Nome = "Notebook Atualizado", Preco = 3999.90m };

            _repositoryMock
                .Setup(r => r.AtualizarProdutoAsync(It.IsAny<Product>()))
                .ReturnsAsync(product);

            var result = await _service.AtualizarProdutoAsync(product);

            result.Should().NotBeNull();
            result.Nome.Should().Be("Notebook Atualizado");
            _repositoryMock.Verify(r => r.AtualizarProdutoAsync(product), Times.Once);
        }

        [Fact]
        public async Task DeletarProduto_DeveChamarRepository()
        {
            _repositoryMock
                .Setup(r => r.DeletarProdutoPorIdAsync(1))
                .Returns(Task.CompletedTask);

            await _service.DeletarProdutoPorIdAsync(1);

            _repositoryMock.Verify(r => r.DeletarProdutoPorIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeletarProduto_NaoEncontrado_DeveLancarExcecao()
        {
            _repositoryMock
                .Setup(r => r.DeletarProdutoPorIdAsync(999))
                .ThrowsAsync(new KeyNotFoundException("Produto com ID 999 não encontrado."));

            var act = () => _service.DeletarProdutoPorIdAsync(999);

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }
    }
}
