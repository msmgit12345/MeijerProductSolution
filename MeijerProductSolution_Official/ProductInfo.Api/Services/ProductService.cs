using Microsoft.Extensions.Caching.Memory;
using ProductInfo.Api.Repositories;

namespace ProductInfo.Api.Services;

public sealed class ProductService : IProductService
{
    private const string ProductListCacheKey = "products:list";
    private static readonly TimeSpan ProductListCacheDuration = TimeSpan.FromMinutes(10);

    private readonly IProductRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository repository, IMemoryCache cache, ILogger<ProductService> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ProductSummaryDto>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(ProductListCacheKey, out IReadOnlyList<ProductSummaryDto>? cachedProducts) && cachedProducts is not null)
        {
            return cachedProducts;
        }

        var products = await _repository.GetProductsAsync();
        _cache.Set(ProductListCacheKey, products, ProductListCacheDuration);
        _logger.LogInformation("Cached {ProductCount} products for {CacheDuration} minutes.", products.Count, ProductListCacheDuration.TotalMinutes);
        return products;
    }

    public Task<ProductDetailDto?> GetProductDetailAsync(int id, CancellationToken cancellationToken = default)
    {
        return _repository.GetProductDetailAsync(id);
    }
}
