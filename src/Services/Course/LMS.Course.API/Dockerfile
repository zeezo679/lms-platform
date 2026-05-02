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

# Course Service layers
COPY src/Services/Course/LMS.Course.API/LMS.Course.API.csproj src/Services/Course/LMS.Course.API/
COPY src/Services/Course/LMS.Course.Application/LMS.Course.Application.csproj src/Services/Course/LMS.Course.Application/
COPY src/Services/Course/LMS.Course.Domain/LMS.Course.Domain.csproj src/Services/Course/LMS.Course.Domain/
COPY src/Services/Course/LMS.Course.Infrastructure/LMS.Course.Infrastructure.csproj src/Services/Course/LMS.Course.Infrastructure/

# Restore (cached unless .csproj files change)
RUN dotnet restore src/Services/Course/LMS.Course.API/LMS.Course.API.csproj

# Copy all source code
COPY src/BuildingBlocks/ src/BuildingBlocks/
COPY src/Services/Course/ src/Services/Course/

# Build and publish
RUN dotnet publish src/Services/Course/LMS.Course.API/LMS.Course.API.csproj \
    -c Release \
    -o /app/publish 

# ================================
# Stage 2: Runtime
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "LMS.Course.API.dll"]