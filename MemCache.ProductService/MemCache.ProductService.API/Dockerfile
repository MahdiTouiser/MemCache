# Use the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the necessary .csproj files
COPY ["MemCache.ProductService.API/MemCache.ProductService.API.csproj", "MemCache.ProductService.API/"]
COPY ["MemCache.ProductService.Contracts/MemCache.ProductService.Contracts.csproj", "MemCache.ProductService.Contracts/"]
COPY ["MemCache.ProductService.Data/MemCache.ProductService.Data.csproj", "MemCache.ProductService.Data/"]

# Restore dependencies
RUN dotnet restore "MemCache.ProductService.API/MemCache.ProductService.API.csproj"

# Copy the rest of the code
COPY . .

# Set the working directory and build the project
WORKDIR "/src/MemCache.ProductService.API"
RUN dotnet build "MemCache.ProductService.API.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "MemCache.ProductService.API.csproj" -c Release -o /app/publish

# Final stage: run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MemCache.ProductService.API.dll"]
