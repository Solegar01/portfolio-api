# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Salin file csproj & restore dependencies dulu
COPY PortfolioApi/*.csproj ./PortfolioApi/
RUN dotnet restore ./PortfolioApi/PortfolioApi.csproj --no-cache

# Copy semua isi repo
COPY . .

# Publish project ke folder publish
RUN dotnet publish ./PortfolioApi/PortfolioApi.csproj -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:${PORT}
EXPOSE 8080

ENTRYPOINT ["dotnet", "PortfolioApi.dll"]
