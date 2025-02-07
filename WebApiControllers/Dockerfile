#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This Dockerfile demonstrates using multiple images to build and finally publish an image using the dotnet SDK and runtime

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["WebApiControllers/WebApiControllers.csproj", "WebApiControllers/WebApiControllers/"]
COPY ["WebApiControllers.DomainLogic/WebApiControllers.DomainLogic.csproj", "WebApiControllers.DomainLogic/WebApiControllers.DomainLogic/"]
COPY ["WebApiControllers.Infrastructure/WebApiControllers.Infrastructure.csproj", "WebApiControllers.Infrastructure/WebApiControllers.Infrastructure/"]
COPY ["WebApiControllers.Shared/WebApiControllers.Shared.csproj", "WebApiControllers.Shared/WebApiControllers.Shared/"]

RUN dotnet restore "./WebApiControllers/WebApiControllers/WebApiControllers.csproj"

COPY . WebApiControllers/
WORKDIR "/src/WebApiControllers/WebApiControllers"

RUN dotnet build "./WebApiControllers.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApiControllers.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApiControllers.dll"]