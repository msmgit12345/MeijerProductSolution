using ProductInfo.Mobile.Models;

namespace ProductInfo.Mobile.Services;

public interface IProductApiService
{
    Task<IReadOnlyList<ProductSummary>> GetProductsAsync(CancellationToken cancellationToken = default);
    Task<ProductDetail?> GetProductDetailAsync(int id, CancellationToken cancellationToken = default);
}
