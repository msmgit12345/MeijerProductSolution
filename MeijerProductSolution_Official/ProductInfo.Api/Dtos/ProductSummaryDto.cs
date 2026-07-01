namespace ProductInfo.Api;

public sealed class ProductSummaryDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
}
