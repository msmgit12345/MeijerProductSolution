using ProductInfo.Mobile.ViewModels;

namespace ProductInfo.Mobile.Views;

public partial class ProductListPage : ContentPage
{
    private readonly ProductListViewModel _viewModel;

    public ProductListPage(ProductListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.Products.Count == 0)
            await _viewModel.LoadProductsAsync();
    }
}
