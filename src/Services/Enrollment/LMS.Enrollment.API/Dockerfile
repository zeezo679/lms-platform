# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Enrollment/LMS.Enrollment.API/LMS.Enrollment.API.csproj", "src/Services/Enrollment/LMS.Enrollment.API/"]
COPY ["src/BuildingBlocks/LMS.Common/LMS.Common.csproj", "src/BuildingBlocks/LMS.Common/"]
COPY ["src/Services/Enrollment/LMS.Enrolment.Application/LMS.Enrollment.Application.csproj", "src/Services/Enrollment/LMS.Enrolment.Application/"]
COPY ["src/BuildingBlocks/LMS.EventBus/LMS.EventBus.csproj", "src/BuildingBlocks/LMS.EventBus/"]
COPY ["src/BuildingBlocks/LMS.Contracts/LMS.Contracts.csproj", "src/BuildingBlocks/LMS.Contracts/"]
COPY ["src/Services/Enrollment/LMS.Enrolment.Domain/LMS.Enrollment.Domain.csproj", "src/Services/Enrollment/LMS.Enrolment.Domain/"]
COPY ["src/Services/Enrollment/LMS.Enrolment.Infrastructure/LMS.Enrollment.Infrastructure.csproj", "src/Services/Enrollment/LMS.Enrolment.Infrastructure/"]
RUN dotnet restore "./src/Services/Enrollment/LMS.Enrollment.API/LMS.Enrollment.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Enrollment/LMS.Enrollment.API"
RUN dotnet build "./LMS.Enrollment.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LMS.Enrollment.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LMS.Enrollment.API.dll"]