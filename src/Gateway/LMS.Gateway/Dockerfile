# ================================
# Stage 1: Build
# ================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project file
COPY src/Gateway/LMS.Gateway/LMS.Gateway.csproj src/Gateway/LMS.Gateway/

# Restore
RUN dotnet restore src/Gateway/LMS.Gateway/LMS.Gateway.csproj

# Copy source
COPY src/Gateway/LMS.Gateway/ src/Gateway/LMS.Gateway/

# Build and publish
RUN dotnet publish src/Gateway/LMS.Gateway/LMS.Gateway.csproj \
    -c Release \
    -o /app/publish

# ================================
# Stage 2: Runtime
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "LMS.Gateway.dll"]