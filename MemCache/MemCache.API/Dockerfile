# Use the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the necessary .csproj files
COPY ["MemCache.API/MemCache.API.csproj", "MemCache.API/"]
COPY ["MemCache.Services/MemCache.Services.csproj", "MemCache.Services/"]
COPY ["MemCache.Contracts/MemCache.Contracts.csproj", "MemCache.Contracts/"]
COPY ["MemCache.Infrastructure/MemCache.Infrastructure.csproj", "MemCache.Infrastructure/"]
COPY ["MemCache.Domain/MemCache.Domain.csproj", "MemCache.Domain/"]

# Restore dependencies
RUN dotnet restore "MemCache.API/MemCache.API.csproj"

# Copy the rest of the code
COPY . .

# Set the working directory and build the project
WORKDIR "/src/MemCache.API"
RUN dotnet build "MemCache.API.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "MemCache.API.csproj" -c Release -o /app/publish

# Final stage: run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MemCache.API.dll"]
