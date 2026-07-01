using System.ComponentModel.DataAnnotations;

namespace ProductInfo.Api.Models;

public sealed class Product
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string Summary { get; set; } = string.Empty;

    [Required, MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Price { get; set; } = string.Empty;

    [Required, MaxLength(1000)]
    public string ImageUrl { get; set; } = string.Empty;
}
