namespace ProductInfo.Mobile.Models;

public sealed class ProductDetail
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}
