using System.Collections.ObjectModel;
using System.Windows.Input;
using ProductInfo.Mobile.Models;
using ProductInfo.Mobile.Services;
using ProductInfo.Mobile.Views;

namespace ProductInfo.Mobile.ViewModels;

public sealed class ProductListViewModel : BaseViewModel
{
    private readonly IProductApiService _productApiService;
    private string _errorMessage = string.Empty;

    public ProductListViewModel(IProductApiService productApiService)
    {
        _productApiService = productApiService;
        Title = "Product List";
        LoadProductsCommand = new Command(async () => await LoadProductsAsync());
        OpenProductCommand = new Command<ProductSummary>(async product => await OpenProductAsync(product));
    }

    public ObservableCollection<ProductSummary> Products { get; } = [];

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public ICommand LoadProductsCommand { get; }
    public ICommand OpenProductCommand { get; }

    public async Task LoadProductsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            Products.Clear();

            var products = await _productApiService.GetProductsAsync();
            foreach (var product in products)
                Products.Add(product);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Unable to load products. Confirm the API is running at http://localhost:5000. Details: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static async Task OpenProductAsync(ProductSummary? product)
    {
        if (product is null)
            return;

        await Shell.Current.GoToAsync($"{nameof(ProductDetailPage)}?productId={product.Id}");
    }
}
