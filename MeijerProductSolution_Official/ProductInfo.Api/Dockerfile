# Build from the repository root:
# docker build -f ProductInfo.Api/Dockerfile -t productinfo-api .
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ProductInfo.Api/ProductInfo.Api.csproj ProductInfo.Api/
RUN dotnet restore ProductInfo.Api/ProductInfo.Api.csproj

COPY ProductInfo.Api/ ProductInfo.Api/
WORKDIR /src/ProductInfo.Api
RUN dotnet publish ProductInfo.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProductInfo.Api.dll"]
