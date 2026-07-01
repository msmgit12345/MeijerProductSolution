# Meijer Product Information Solution

This is a clean rebuild using the standard .NET 8 project structure:

- `ProductInfo.Api` - ASP.NET Core 8 REST API with Swagger
- `ProductInfo.Mobile` - .NET MAUI Android app using MVVM
- One solution file only: `MeijerProductSolution.sln`

The API uses the supplied JSON files as the persistence/data layer:

- `ProductInfo.Api/Data/products.json`
- `ProductInfo.Api/Data/product-details.json`

## Prerequisites

Install these before running:

1. Visual Studio 2022 with the **.NET Multi-platform App UI development** workload
2. .NET 8 SDK
3. Android Emulator from Visual Studio Android Device Manager
4. Internet access for first NuGet restore

If NuGet restore fails, confirm this file exists in the solution root:

```text
NuGet.Config
```

It points restore to `https://api.nuget.org/v3/index.json`.

## Run the API

1. Open `MeijerProductSolution.sln` in Visual Studio.
2. Right-click `ProductInfo.Api`.
3. Select **Set as Startup Project**.
4. Press **F5**.
5. Swagger should open at:

```text
http://localhost:5000/swagger
```

API endpoints:

```text
GET http://localhost:5000/api/products
GET http://localhost:5000/api/products/0
```

Leave the API running while testing the mobile app.

## Run the MAUI Android App

1. Start the Android emulator from **Tools > Android > Android Device Manager**.
2. Wait until the emulator is fully booted and you can see the Android home screen.
3. In Visual Studio, right-click `ProductInfo.Mobile`.
4. Select **Set as Startup Project**.
5. In the run target dropdown, select the Android emulator, for example:

```text
Pixel 5 - API 34
```

6. Press **F5**.

The app should show the product list. Tapping a product opens the detail screen.

## Important Android API URL

The MAUI app uses this API base URL on Android:

```text
http://10.0.2.2:5000
```

Android emulators cannot call the host machine API using `localhost`. `10.0.2.2` is the Android emulator alias for the host computer.

## If Visual Studio deploy hangs

Open a command prompt and run:

```cmd
cd "C:\Program Files (x86)\Android\android-sdk\platform-tools"
adb devices
```

Expected output:

```text
emulator-5554    device
```

Then from the solution folder:

```cmd
dotnet build ProductInfo.Mobile\ProductInfo.Mobile.csproj -f net8.0-android
```

If APK files are created under:

```text
ProductInfo.Mobile\bin\Debug\net8.0-android
```

install manually:

```cmd
cd "C:\Program Files (x86)\Android\android-sdk\platform-tools"
adb install -r "C:\Path\To\MeijerProductSolution_Official\ProductInfo.Mobile\bin\Debug\net8.0-android\com.companyname.productinfo-Signed.apk"
adb shell monkey -p com.companyname.productinfo -c android.intent.category.LAUNCHER 1
```

## Share / Add to List

On the product detail page, tap **Add to List / Share**. The app creates this message:

```text
{product title} - {price} from {city} added to list
```

Example:

```text
Bananas - $0.59/lb from Chicago added to list
```

The app asks for location permission. If permission is denied or unavailable, it uses `Unknown City`.

## Project Notes

- MVVM is implemented without external MVVM packages to reduce restore issues.
- Swagger is enabled for the API.
- Android manifest includes Internet and Location permissions.
- Cleartext HTTP is enabled for Android local API testing.
- Package id: `com.companyname.productinfo`
