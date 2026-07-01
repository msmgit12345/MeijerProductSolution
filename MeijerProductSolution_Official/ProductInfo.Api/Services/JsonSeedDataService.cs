using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ProductInfo.Api.Data;
using ProductInfo.Api.Models;

namespace ProductInfo.Api.Services;

public sealed class JsonSeedDataService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<JsonSeedDataService> _logger;

    public JsonSeedDataService(IServiceProvider serviceProvider, IConfiguration configuration, IWebHostEnvironment environment, ILogger<JsonSeedDataService> logger)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _environment = environment;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!string.Equals(_configuration["DataSource:Provider"], "SqlServer", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        await using var scope = _serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

        if (_configuration.GetValue("DataSource:AutoMigrate", false))
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        }

        if (await dbContext.Products.AnyAsync(cancellationToken))
        {
            return;
        }

        var seedPath = Path.Combine(_environment.ContentRootPath, "Data", "product-details.json");
        await using var stream = File.OpenRead(seedPath);
        var products = await JsonSerializer.DeserializeAsync<List<Product>>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken) ?? [];

        dbContext.Products.AddRange(products);
        await dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Seeded SQL Server database with {ProductCount} products from JSON.", products.Count);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
