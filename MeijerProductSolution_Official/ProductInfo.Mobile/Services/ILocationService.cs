namespace ProductInfo.Mobile.Services;

public interface ILocationService
{
    Task<string> GetCurrentCityAsync();
}
