# ─────────────────────────────────────────────────────────────────────────────
# Stage 1 — Build
# ─────────────────────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# ── Copy project files first for optimal layer caching ────────────────────────
# Docker will only re-run dotnet restore when a .csproj file changes,
# not on every source code change.

COPY ["src/Services/User/API/API.csproj",                         "src/Services/User/API/"]
COPY ["src/Services/User/Application/Application.csproj",         "src/Services/User/Application/"]
COPY ["src/Services/User/Infrastructure/Infrastructure.csproj",   "src/Services/User/Infrastructure/"]
COPY ["src/Services/User/Domain/Domain.csproj",                   "src/Services/User/Domain/"]
COPY ["src/BuildingBlocks/LMS.Common/LMS.Common.csproj",                             "src/BuildingBlocks/LMS.Common/"]
COPY ["src/BuildingBlocks/LMS.Contracts/LMS.Contracts.csproj",                       "src/BuildingBlocks/LMS.Contracts/"]
COPY ["src/BuildingBlocks/LMS.EventBus/LMS.EventBus.csproj",                         "src/BuildingBlocks/LMS.EventBus/"]

RUN dotnet restore "src/Services/User/API/API.csproj"

# ── Copy the full source and publish ──────────────────────────────────────────
COPY . .

RUN dotnet publish "src/Services/User/API/API.csproj" \
    --configuration Release \
    --no-restore \
    --output /app/publish \
    /p:UseAppHost=false

# ─────────────────────────────────────────────────────────────────────────────
# Stage 2 — Runtime
# ─────────────────────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Create a non-root user — never run containers as root in production
RUN addgroup --system appgroup && \
    adduser --system --ingroup appgroup appuser

# Copy only the published output from the build stage
COPY --from=build /app/publish .

# Hand ownership to the non-root user
RUN chown -R appuser:appgroup /app

USER appuser

# Kestrel will bind to this port inside the container.
# docker-compose maps 5005:8080 externally.
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "API.dll"]