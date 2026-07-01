using Microsoft.EntityFrameworkCore;
using ProductInfo.Api.Data;

namespace ProductInfo.Api.Repositories;

public sealed class SqlProductRepository : IProductRepository
{
    private readonly ProductDbContext _dbContext;

    public SqlProductRepository(ProductDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ProductSummaryDto>> GetProductsAsync()
    {
        return await _dbContext.Products
            .AsNoTracking()
            .OrderBy(product => product.Id)
            .Select(product => new ProductSummaryDto
            {
                Id = product.Id,
                Title = product.Title,
                Summary = product.Summary,
                ImageUrl = product.ImageUrl
            })
            .ToListAsync();
    }

    public async Task<ProductDetailDto?> GetProductDetailAsync(int id)
    {
        return await _dbContext.Products
            .AsNoTracking()
            .Where(product => product.Id == id)
            .Select(product => new ProductDetailDto
            {
                Id = product.Id,
                Title = product.Title,
                Summary = product.Summary,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl
            })
            .FirstOrDefaultAsync();
    }
}
