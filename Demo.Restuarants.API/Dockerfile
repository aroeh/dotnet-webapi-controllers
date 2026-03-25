FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Demo.Restuarants.API/Demo.Restuarants.API.csproj", "Demo.Restuarants.API/Demo.Restuarants.API/"]
COPY ["Demo.Restuarants.Core/Demo.Restuarants.Core.csproj", "Demo.Restuarants.Core/Demo.Restuarants.Core/"]
COPY ["Demo.Restuarants.Infrastructure.MongoDb/Demo.Restuarants.Infrastructure.MongoDb.csproj", "Demo.Restuarants.Infrastructure.MongoDb/Demo.Restuarants.Infrastructure.MongoDb/"]
COPY ["Demo.Restuarants.Shared/Demo.Restuarants.Shared.csproj", "Demo.Restuarants.Shared/Demo.Restuarants.Shared/"]

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