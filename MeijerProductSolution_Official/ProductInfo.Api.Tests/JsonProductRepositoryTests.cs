using ProductInfo.Api.Repositories;

namespace ProductInfo.Api.Tests;

public sealed class JsonProductRepositoryTests
{
    [Fact]
    public async Task GetProductsAsync_Returns_ProductSummaries()
    {
        var repository = new JsonProductRepository(new FakeWebHostEnvironment());

        var products = await repository.GetProductsAsync();

        Assert.NotEmpty(products);
        Assert.Contains(products, product => product.Id == 0 && product.Title == "Bananas");
    }

    [Fact]
    public async Task GetProductDetailAsync_Returns_Detail_WhenProductExists()
    {
        var repository = new JsonProductRepository(new FakeWebHostEnvironment());

        var product = await repository.GetProductDetailAsync(0);

        Assert.NotNull(product);
        Assert.Equal("Bananas", product.Title);
        Assert.Equal("$0.59/lb", product.Price);
    }

    [Fact]
    public async Task GetProductDetailAsync_Returns_Null_WhenProductDoesNotExist()
    {
        var repository = new JsonProductRepository(new FakeWebHostEnvironment());

        var product = await repository.GetProductDetailAsync(9999);

        Assert.Null(product);
    }
}
