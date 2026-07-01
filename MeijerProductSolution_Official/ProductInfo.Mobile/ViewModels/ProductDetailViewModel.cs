using System.Windows.Input;
using ProductInfo.Mobile.Models;
using ProductInfo.Mobile.Services;

namespace ProductInfo.Mobile.ViewModels;

[QueryProperty(nameof(ProductId), "productId")]
public sealed class ProductDetailViewModel : BaseViewModel
{
    private readonly IProductApiService _productApiService;
    private readonly ILocationService _locationService;
    private int _productId;
    private ProductDetail? _product;
    private string _errorMessage = string.Empty;

    public ProductDetailViewModel(IProductApiService productApiService, ILocationService locationService)
    {
        _productApiService = productApiService;
        _locationService = locationService;
        Title = "Product Detail";
        ShareCommand = new Command(async () => await ShareProductAsync(), () => Product is not null && !IsBusy);
    }

    public int ProductId
    {
        get => _productId;
        set
        {
            if (SetProperty(ref _productId, value))
                _ = LoadProductAsync(value);
        }
    }

    public ProductDetail? Product
    {
        get => _product;
        set
        {
            SetProperty(ref _product, value);
            OnPropertyChanged(nameof(HasProduct));
            ((Command)ShareCommand).ChangeCanExecute();
        }
    }

    public bool HasProduct => Product is not null;

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public ICommand ShareCommand { get; }

    private async Task LoadProductAsync(int productId)
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            Product = await _productApiService.GetProductDetailAsync(productId);
            Title = Product?.Title ?? "Product Detail";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Unable to load product details. Details: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
            ((Command)ShareCommand).ChangeCanExecute();
        }
    }

    private async Task ShareProductAsync()
    {
        if (Product is null)
            return;

        var city = await _locationService.GetCurrentCityAsync();
        var message = $"{Product.Title} - {Product.Price} from {city} added to list";

        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Text = message,
            Title = "Add to list"
        });
    }
}
