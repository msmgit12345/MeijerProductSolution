using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductInfo.Api;
using ProductInfo.Api.Data;
using ProductInfo.Api.Repositories;
using ProductInfo.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls(builder.Configuration["ASPNETCORE_URLS"] ?? "http://0.0.0.0:5000");

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

builder.Services.AddMemoryCache();
builder.Services.AddScoped<IProductService, ProductService>();

var dataProvider = builder.Configuration["DataSource:Provider"] ?? "Json";
if (string.Equals(dataProvider, "SqlServer", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddDbContext<ProductDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ProductDb")));
    builder.Services.AddScoped<IProductRepository, SqlProductRepository>();
    builder.Services.AddHostedService<JsonSeedDataService>();
}
else
{
    builder.Services.AddSingleton<IProductRepository, JsonProductRepository>();
}

var app = builder.Build();

app.UseExceptionHandler(exceptionApp =>
{
    exceptionApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred while processing the request." });
    });
});

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Information API v1");
    options.RoutePrefix = "swagger";
});

app.UseCors();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapGet("/health", () => Results.Ok(new { status = "Healthy", provider = dataProvider, timeUtc = DateTimeOffset.UtcNow }))
    .WithName("HealthCheck");

app.MapGet("/api/products", async (IProductService productService, CancellationToken cancellationToken) =>
{
    var products = await productService.GetProductsAsync(cancellationToken);
    return Results.Ok(products);
})
.WithName("GetProducts")
.Produces<IReadOnlyList<ProductSummaryDto>>(StatusCodes.Status200OK);

app.MapGet("/api/products/{id:int}", async (int id, IProductService productService, CancellationToken cancellationToken) =>
{
    var product = await productService.GetProductDetailAsync(id, cancellationToken);
    return product is null ? Results.NotFound(new { error = $"Product with id {id} was not found." }) : Results.Ok(product);
})
.WithName("GetProductById")
.Produces<ProductDetailDto>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

app.Run();

public partial class Program
{
}
