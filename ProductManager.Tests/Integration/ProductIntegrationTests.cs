using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProductManager.API;
using ProductManager.Infra.Interfaces;
using ProductManager.Infra.Models;
using ProductManager.Infra.Repository;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ProductManager.Tests.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptors = services.Where(d =>
                    d.ServiceType == typeof(ProductContext) ||
                    d.ServiceType == typeof(DbContextOptions<ProductContext>) ||
                    d.ServiceType == typeof(IProductRepository)).ToList();

                foreach (var descriptor in descriptors)
                    services.Remove(descriptor);

                var allDbOptions = services.Where(d =>
                    d.ServiceType.FullName != null &&
                    d.ServiceType.FullName.Contains("DbContextOptions")).ToList();

                foreach (var descriptor in allDbOptions)
                    services.Remove(descriptor);

                services.AddDbContext<ProductContext>(options =>
                    options.UseInMemoryDatabase(databaseName: "TestDb_Integration"));

                services.AddScoped<IProductRepository, ProductRepository>();
            });
        }
    }

    public class ProductIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public ProductIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        private async Task SeedDatabase()
        {
            await using var scope = _factory.Server.Services.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<ProductContext>();
            await db.Database.EnsureCreatedAsync();

            if (!await db.Products.AnyAsync())
            {
                db.Products.AddRange(
                    new Core.Models.Product { Id = 1, Nome = "Notebook Dell Inspiron 15", Preco = 4599.90m },
                    new Core.Models.Product { Id = 2, Nome = "Mouse Logitech MX Master 3S", Preco = 349.90m },
                    new Core.Models.Product { Id = 3, Nome = "Teclado Mecânico Keychron K2", Preco = 499.90m },
                    new Core.Models.Product { Id = 4, Nome = "Monitor LG UltraWide 29\"", Preco = 1899.90m },
                    new Core.Models.Product { Id = 5, Nome = "Webcam Logitech C920", Preco = 299.90m }
                );
                await db.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task GET_Product_DeveRetornarOkComLista()
        {
            await SeedDatabase();
            var response = await _client.GetAsync("/Product");
            var content = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK, because: content);

            var json = JsonSerializer.Deserialize<JsonElement>(content);
            json.GetProperty("items").ValueKind.Should().Be(JsonValueKind.Array);
            json.GetProperty("totalCount").GetInt32().Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GET_Product_IdValido_DeveRetornarOk()
        {
            await SeedDatabase();
            var response = await _client.GetAsync("/Product/1");
            var content = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK, because: content);

            var json = JsonSerializer.Deserialize<JsonElement>(content);
            json.GetProperty("id").GetInt32().Should().Be(1);
        }

        [Fact]
        public async Task GET_Product_IdInvalido_DeveRetornarNotFound()
        {
            var response = await _client.GetAsync("/Product/9999");
            var content = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.NotFound, because: content);
        }

        [Fact]
        public async Task POST_Product_DeveCriarProduto()
        {
            var product = new { nome = "Produto Teste", preco = 49.90 };

            var postResponse = await _client.PostAsJsonAsync("/Product", product);
            var content = await postResponse.Content.ReadAsStringAsync();

            postResponse.StatusCode.Should().BeOneOf(HttpStatusCode.Created, HttpStatusCode.OK);

            var json = JsonSerializer.Deserialize<JsonElement>(content);
            json.GetProperty("nome").GetString().Should().Be("Produto Teste");
            json.GetProperty("preco").GetDouble().Should().Be(49.90);
        }

        [Fact]
        public async Task PUT_Product_DeveRetornarOk()
        {
            var created = await _client.PostAsJsonAsync("/Product", new { nome = "Produto Para Atualizar", preco = 50.00 });
            var createdJson = JsonSerializer.Deserialize<JsonElement>(await created.Content.ReadAsStringAsync());
            var id = createdJson.GetProperty("id").GetInt32();

            var product = new { nome = "Produto Atualizado", preco = 99.90 };

            var response = await _client.PutAsJsonAsync($"/Product/{id}", product);
            var content = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK, because: content);
        }

        [Fact]
        public async Task DELETE_Product_DeveRetornarNoContent()
        {
            var created = await _client.PostAsJsonAsync("/Product", new { nome = "Produto Para Deletar", preco = 10.00 });
            var createdJson = JsonSerializer.Deserialize<JsonElement>(await created.Content.ReadAsStringAsync());
            var id = createdJson.GetProperty("id").GetInt32();

            var response = await _client.DeleteAsync($"/Product/{id}");
            var content = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().BeOneOf(HttpStatusCode.NoContent, HttpStatusCode.OK);
        }

        [Fact]
        public async Task GET_Product_Paginacao_DeveRetornarDadosCorretos()
        {
            await SeedDatabase();
            var response = await _client.GetAsync("/Product?page=1&pageSize=2");
            var content = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK, because: content);

            var json = JsonSerializer.Deserialize<JsonElement>(content);
            json.GetProperty("items").GetArrayLength().Should().Be(2);
            json.GetProperty("currentPage").GetInt32().Should().Be(1);
        }

        [Fact]
        public async Task POST_Product_DadosInvalidos_DeveRetornarBadRequest()
        {
            var product = new { nome = "", preco = -1 };

            var response = await _client.PostAsJsonAsync("/Product", product);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GET_Product_SegundaPagina_DeveRetornarDados()
        {
            await SeedDatabase();
            var response = await _client.GetAsync("/Product?page=2&pageSize=2");
            var content = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK, because: content);

            var json = JsonSerializer.Deserialize<JsonElement>(content);
            json.GetProperty("currentPage").GetInt32().Should().Be(2);
        }
    }
}
