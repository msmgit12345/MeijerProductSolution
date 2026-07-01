namespace ProductInfo.Api.Repositories;

public interface IProductRepository
{
    Task<IReadOnlyList<ProductSummaryDto>> GetProductsAsync();
    Task<ProductDetailDto?> GetProductDetailAsync(int id);
}
