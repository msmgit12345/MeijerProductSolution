using Microsoft.Extensions.DependencyInjection;
using ProductInfo.Mobile.Services;
using ProductInfo.Mobile.ViewModels;
using ProductInfo.Mobile.Views;

namespace ProductInfo.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>();

        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<IProductApiService, ProductApiService>();
        builder.Services.AddSingleton<ILocationService, LocationService>();
        builder.Services.AddSingleton<ProductListViewModel>();
        builder.Services.AddTransient<ProductDetailViewModel>();
        builder.Services.AddSingleton<ProductListPage>();
        builder.Services.AddTransient<ProductDetailPage>();

        return builder.Build();
    }
}
