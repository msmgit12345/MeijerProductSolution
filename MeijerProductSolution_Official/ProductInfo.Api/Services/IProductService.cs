namespace ProductInfo.Api.Services;

public interface IProductService
{
    Task<IReadOnlyList<ProductSummaryDto>> GetProductsAsync(CancellationToken cancellationToken = default);
    Task<ProductDetailDto?> GetProductDetailAsync(int id, CancellationToken cancellationToken = default);
}
