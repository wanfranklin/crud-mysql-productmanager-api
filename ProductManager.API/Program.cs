using System;
using ProductManager.Domain.Interfaces;
using ProductManager.Domain.Services;
using ProductManager.Infra.Interfaces;
using ProductManager.Infra.Repository;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

var directory = Directory.GetCurrentDirectory();

var logPath = Path.Combine(directory, "log.txt");

if (!Directory.Exists(logPath))
{
    Directory.CreateDirectory(logPath);
}

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(new CompactJsonFormatter(), Path.Combine(logPath), rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();