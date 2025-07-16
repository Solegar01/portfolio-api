# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY PortfolioApi/*.csproj ./PortfolioApi/
RUN dotnet restore ./PortfolioApi/PortfolioApi.csproj

COPY . .
RUN dotnet publish ./PortfolioApi/PortfolioApi.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Railway uses PORT environment variable
ENV ASPNETCORE_URLS=http://+:${PORT}
EXPOSE 8080

ENTRYPOINT ["dotnet", "PortfolioApi.dll"]
