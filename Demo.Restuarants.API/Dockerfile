FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Demo.Restuarants.API/Demo.Restuarants.API.csproj", "Demo.Restuarants.API/Demo.Restuarants.API/"]
COPY ["Demo.Restuarants.API.Core/Demo.Restuarants.API.Core.csproj", "Demo.Restuarants.API.Core/Demo.Restuarants.API.Core/"]
COPY ["Demo.Restuarants.API.MongoDb.Infrastructure/Demo.Restuarants.API.MongoDb.Infrastructure.csproj", "Demo.Restuarants.API.MongoDb.Infrastructure/Demo.Restuarants.API.MongoDb.Infrastructure/"]
COPY ["Demo.Restuarants.API.Shared/Demo.Restuarants.API.Shared.csproj", "Demo.Restuarants.API.Shared/Demo.Restuarants.API.Shared/"]

RUN dotnet restore "./Demo.Restuarants.API/Demo.Restuarants.API/Demo.Restuarants.API.csproj"

COPY . Demo.Restuarants.API/

WORKDIR "/src/Demo.Restuarants.API/Demo.Restuarants.API"

RUN dotnet build "./Demo.Restuarants.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Demo.Restuarants.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Demo.Restuarants.API.dll"]