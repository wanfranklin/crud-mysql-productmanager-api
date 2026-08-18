using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManager.API.Controllers;
using ProductManager.API.Dtos;
using ProductManager.Core.Models;
using ProductManager.Domain.Interfaces;

namespace ProductManager.Tests.Unit.API
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _serviceMock;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _serviceMock = new Mock<IProductService>();
            _controller = new ProductController(_serviceMock.Object);
        }

        [Fact]
        public async Task ObterProdutos_DeveRetornarOkComPagedResult()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Nome = "Notebook", Preco = 4599.90m },
                new Product { Id = 2, Nome = "Mouse", Preco = 349.90m }
            };

            var pagedResult = PagedResult<Product>.Create(products, 2, 1, 10);

            _serviceMock
                .Setup(s => s.ObterProdutosAsync(1, 10))
                .ReturnsAsync(pagedResult);

            var result = await _controller.ObterProdutosAsync(1, 10);

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<PagedResult<ProductResponse>>().Subject;
            response.Items.Should().HaveCount(2);
            response.TotalCount.Should().Be(2);
            response.CurrentPage.Should().Be(1);
        }

        [Fact]
        public async Task ObterProdutos_PageInvalido_DeveUsarPadrao1()
        {
            var pagedResult = PagedResult<Product>.Create(
                Enumerable.Empty<Product>(), 0, 1, 10);

            _serviceMock
                .Setup(s => s.ObterProdutosAsync(1, 10))
                .ReturnsAsync(pagedResult);

            var result = await _controller.ObterProdutosAsync(-5, 10);

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<PagedResult<ProductResponse>>().Subject;
            response.CurrentPage.Should().Be(1);
        }

        [Fact]
        public async Task ObterProdutos_PageSizeInvalido_DeveClampar()
        {
            var pagedResult = PagedResult<Product>.Create(
                Enumerable.Empty<Product>(), 0, 1, 100);

            _serviceMock
                .Setup(s => s.ObterProdutosAsync(1, 100))
                .ReturnsAsync(pagedResult);

            var result = await _controller.ObterProdutosAsync(1, 500);

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<PagedResult<ProductResponse>>().Subject;
            response.PageSize.Should().Be(100);
        }

        [Fact]
        public async Task ObterProdutoPorId_Encontrado_DeveRetornarOk()
        {
            var product = new Product { Id = 1, Nome = "Notebook", Preco = 4599.90m };

            _serviceMock
                .Setup(s => s.ObterProdutoPorIdAsync(1))
                .ReturnsAsync(product);

            var result = await _controller.ObterProdutoPorIdAsync(1);

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ProductResponse>().Subject;
            response.Id.Should().Be(1);
            response.Nome.Should().Be("Notebook");
            response.Preco.Should().Be(4599.90m);
        }

        [Fact]
        public async Task ObterProdutoPorId_NaoEncontrado_DeveLancarExcecao()
        {
            _serviceMock
                .Setup(s => s.ObterProdutoPorIdAsync(999))
                .ThrowsAsync(new KeyNotFoundException("Produto não encontrado."));

            var act = () => _controller.ObterProdutoPorIdAsync(999);

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task CriarProduto_DeveRetornarCreated()
        {
            var request = new CreateProductRequest { Nome = "Fone JBL", Preco = 199.90m };
            var created = new Product { Id = 6, Nome = "Fone JBL", Preco = 199.90m };

            _serviceMock
                .Setup(s => s.CriarProdutoAsync(It.IsAny<Product>()))
                .ReturnsAsync(created);

            var result = await _controller.CriarProdutoAsync(request);

            var createdResult = result.Result.Should().BeOfType<CreatedResult>().Subject;
            var response = createdResult.Value.Should().BeOfType<ProductResponse>().Subject;
            response.Id.Should().Be(6);
            response.Nome.Should().Be("Fone JBL");
        }

        [Fact]
        public async Task AtualizarProduto_DeveRetornarOk()
        {
            var request = new UpdateProductRequest { Nome = "Mouse Atualizado", Preco = 299.90m };
            var updated = new Product { Id = 1, Nome = "Mouse Atualizado", Preco = 299.90m };

            _serviceMock
                .Setup(s => s.AtualizarProdutoAsync(It.IsAny<Product>()))
                .ReturnsAsync(updated);

            var result = await _controller.AtualizarProdutoPorIdAsync(1, request);

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ProductResponse>().Subject;
            response.Nome.Should().Be("Mouse Atualizado");
            response.Preco.Should().Be(299.90m);
        }

        [Fact]
        public async Task DeletarProduto_DeveRetornarNoContent()
        {
            _serviceMock
                .Setup(s => s.DeletarProdutoPorIdAsync(1))
                .Returns(Task.CompletedTask);

            var result = await _controller.DeletarProdutoPorId(1);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeletarProduto_NaoEncontrado_DeveLancarExcecao()
        {
            _serviceMock
                .Setup(s => s.DeletarProdutoPorIdAsync(999))
                .ThrowsAsync(new KeyNotFoundException("Produto não encontrado."));

            var act = () => _controller.DeletarProdutoPorId(999);

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }
    }
}
