using System.Net;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using ProductManager.API.Middleware;

namespace ProductManager.Tests.Unit.API
{
    public class ExceptionMiddlewareTests
    {
        private readonly ExceptionMiddleware _middleware;

        public ExceptionMiddlewareTests()
        {
            _middleware = new ExceptionMiddleware(context => Task.CompletedTask);
        }

        [Fact]
        public async Task KeyNotFoundException_DeveRetornar404()
        {
            var middleware = new ExceptionMiddleware(_ => throw new KeyNotFoundException("Produto não encontrado."));
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            await middleware.InvokeAsync(context);

            context.Response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ArgumentException_DeveRetornar400()
        {
            var middleware = new ExceptionMiddleware(_ => throw new ArgumentException("Dado inválido."));
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            await middleware.InvokeAsync(context);

            context.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UnauthorizedAccessException_DeveRetornar401()
        {
            var middleware = new ExceptionMiddleware(_ => throw new UnauthorizedAccessException());
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            await middleware.InvokeAsync(context);

            context.Response.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ExcecaoGenerica_DeveRetornar500()
        {
            var middleware = new ExceptionMiddleware(_ => throw new InvalidOperationException("Erro inesperado."));
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            await middleware.InvokeAsync(context);

            context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task SemExcecao_DeveRetornarStatusCodeOk()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            await _middleware.InvokeAsync(context);

            context.Response.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Resposta_DeveSerJsonComCamelCase()
        {
            var middleware = new ExceptionMiddleware(_ => throw new KeyNotFoundException("Produto não encontrado."));
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            await middleware.InvokeAsync(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var json = JsonSerializer.Deserialize<JsonElement>(body);

            json.GetProperty("statusCode").GetInt32().Should().Be(404);
            json.GetProperty("message").GetString().Should().Contain("Produto não encontrado");
            json.TryGetProperty("timestamp", out _).Should().BeTrue();
        }
    }
}
