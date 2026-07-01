using System.Text.Json;

namespace ProductInfo.Api.Repositories;

public sealed class JsonProductRepository : IProductRepository
{
    private readonly IWebHostEnvironment _environment;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public JsonProductRepository(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<IReadOnlyList<ProductSummaryDto>> GetProductsAsync()
    {
        var path = Path.Combine(_environment.ContentRootPath, "Data", "products.json");
        await using var stream = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<List<ProductSummaryDto>>(stream, _jsonOptions) ?? [];
    }

    public async Task<ProductDetailDto?> GetProductDetailAsync(int id)
    {
        var path = Path.Combine(_environment.ContentRootPath, "Data", "product-details.json");
        await using var stream = File.OpenRead(path);
        var products = await JsonSerializer.DeserializeAsync<List<ProductDetailDto>>(stream, _jsonOptions) ?? [];
        return products.FirstOrDefault(product => product.Id == id);
    }
}
