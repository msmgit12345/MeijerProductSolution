using System.Net.Http.Json;
using ProductInfo.Mobile.Models;

namespace ProductInfo.Mobile.Services;

public sealed class ProductApiService : IProductApiService
{
    private readonly HttpClient _httpClient;

    public ProductApiService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(DeviceInfo.Platform == DevicePlatform.Android
                ? "http://10.0.2.2:5000"
                : "http://localhost:5000")
        };
    }

    public async Task<IReadOnlyList<ProductSummary>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await _httpClient.GetFromJsonAsync<List<ProductSummary>>("/api/products", cancellationToken);
        return products ?? [];
    }

    public async Task<ProductDetail?> GetProductDetailAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<ProductDetail>($"/api/products/{id}", cancellationToken);
    }
}
