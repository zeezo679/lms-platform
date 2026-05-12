# ================================
# Stage 1: Build
# ================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files first (layer cache optimization)
# BuildingBlocks
COPY src/BuildingBlocks/LMS.Common/LMS.Common.csproj src/BuildingBlocks/LMS.Common/
COPY src/BuildingBlocks/LMS.Contracts/LMS.Contracts.csproj src/BuildingBlocks/LMS.Contracts/
COPY src/BuildingBlocks/LMS.EventBus/LMS.EventBus.csproj src/BuildingBlocks/LMS.EventBus/

# Auth Service layers
COPY src/Services/Auth/Domain/Domain.csproj src/Services/Auth/Domain/
COPY src/Services/Auth/Application/Application.csproj src/Services/Auth/Application/
COPY src/Services/Auth/Infrastructure/Infrastructure.csproj src/Services/Auth/Infrastructure/
COPY src/Services/Auth/LMS.Auth.API/LMS.Auth.API.csproj src/Services/Auth/LMS.Auth.API/

# Restore (cached unless .csproj files change)
RUN dotnet restore src/Services/Auth/LMS.Auth.API/LMS.Auth.API.csproj

# Copy all source code
COPY src/BuildingBlocks/ src/BuildingBlocks/
COPY src/Services/Auth/ src/Services/Auth/

# Build and publish
RUN dotnet publish src/Services/Auth/LMS.Auth.API/LMS.Auth.API.csproj \
    -c Release \
    -o /app/publish 

# ================================
# Stage 2: Runtime
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "LMS.Auth.API.dll"]