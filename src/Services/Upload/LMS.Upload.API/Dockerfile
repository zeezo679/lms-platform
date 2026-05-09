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

# Upload Service layers
COPY src/Services/Upload/LMS.Upload.Domain/LMS.Upload.Domain.csproj src/Services/Upload/LMS.Upload.Domain/
COPY src/Services/Upload/LMS.Upload.Application/LMS.Upload.Application.csproj src/Services/Upload/LMS.Upload.Application/
COPY src/Services/Upload/LMS.Upload.Infrastructure/LMS.Upload.Infrastructure.csproj src/Services/Upload/LMS.Upload.Infrastructure/
COPY src/Services/Upload/LMS.Upload.API/LMS.Upload.API.csproj src/Services/Upload/LMS.Upload.API/

# Restore (cached unless .csproj files change)
RUN dotnet restore src/Services/Upload/LMS.Upload.API/LMS.Upload.API.csproj

# Copy all source code
COPY src/BuildingBlocks/ src/BuildingBlocks/
COPY src/Services/Upload/ src/Services/Upload/

# Build and publish
RUN dotnet publish src/Services/Upload/LMS.Upload.API/LMS.Upload.API.csproj \
    -c Release \
    -o /app/publish 

# ================================
# Stage 2: Runtime
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "LMS.Upload.API.dll"]
