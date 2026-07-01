using Microsoft.OpenApi.Models;
using ProductInfo.Api;
using ProductInfo.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:5000");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product Information API",
        Version = "v1",
        Description = "API for retrieving product summaries and product details for the MAUI mobile app."
    });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddSingleton<IProductRepository, JsonProductRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Information API v1");
    options.RoutePrefix = "swagger";
});

app.UseCors();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapGet("/api/products", async (IProductRepository repository) =>
{
    var products = await repository.GetProductsAsync();
    return Results.Ok(products);
})
.WithName("GetProducts")
.Produces<IReadOnlyList<ProductSummaryDto>>(StatusCodes.Status200OK);

app.MapGet("/api/products/{id:int}", async (int id, IProductRepository repository) =>
{
    var product = await repository.GetProductDetailAsync(id);
    return product is null ? Results.NotFound($"Product with id {id} was not found.") : Results.Ok(product);
})
.WithName("GetProductById")
.Produces<ProductDetailDto>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

app.Run();
