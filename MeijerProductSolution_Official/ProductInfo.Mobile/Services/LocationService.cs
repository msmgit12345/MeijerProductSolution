namespace ProductInfo.Mobile.Services;

public sealed class LocationService : ILocationService
{
    public async Task<string> GetCurrentCityAsync()
    {
        try
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
                return "Unknown City";

            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var location = await Geolocation.Default.GetLocationAsync(request);

            if (location is null)
                return "Unknown City";

            var placemarks = await Geocoding.Default.GetPlacemarksAsync(location.Latitude, location.Longitude);
            var placemark = placemarks?.FirstOrDefault();

            return string.IsNullOrWhiteSpace(placemark?.Locality)
                ? "Unknown City"
                : placemark.Locality;
        }
        catch
        {
            return "Unknown City";
        }
    }
}
