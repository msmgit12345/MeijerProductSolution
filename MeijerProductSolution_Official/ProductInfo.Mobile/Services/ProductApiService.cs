using System.Net.Http.Json;
using ProductInfo.Mobile.Models;

namespace ProductInfo.Mobile.Services;

public sealed class ProductApiService : IProductApiService
{
    private static readonly TimeSpan ProductListCacheDuration = TimeSpan.FromMinutes(5);
    private readonly HttpClient _httpClient;
    private IReadOnlyList<ProductSummary>? _cachedProducts;
    private DateTimeOffset _cachedProductsUntilUtc;

    public ProductApiService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(DeviceInfo.Platform == DevicePlatform.Android
                ? "http://10.0.2.2:5000"
                : "http://localhost:5000"),
            Timeout = TimeSpan.FromSeconds(15)
        };
    }

    public async Task<IReadOnlyList<ProductSummary>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        if (_cachedProducts is not null && DateTimeOffset.UtcNow < _cachedProductsUntilUtc)
        {
            return _cachedProducts;
        }

        using var response = await _httpClient.GetAsync("/api/products", cancellationToken);
        response.EnsureSuccessStatusCode();

        var products = await response.Content.ReadFromJsonAsync<List<ProductSummary>>(cancellationToken: cancellationToken) ?? [];
        _cachedProducts = products;
        _cachedProductsUntilUtc = DateTimeOffset.UtcNow.Add(ProductListCacheDuration);
        return products;
    }

    public async Task<ProductDetail?> GetProductDetailAsync(int id, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync($"/api/products/{id}", cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ProductDetail>(cancellationToken: cancellationToken);
    }
}
