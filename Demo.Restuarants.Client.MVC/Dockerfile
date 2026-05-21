FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Demo.Restuarants.Client.MVC/Demo.Restuarants.Client.MVC.csproj", "Demo.Restuarants.Client.MVC/"]
COPY ["Demo.Restuarants.API.SDK/Demo.Restuarants.API.SDK.csproj", "Demo.Restuarants.API.SDK/"]
COPY ["Demo.Restuarants.API.Models/Demo.Restuarants.API.Models.csproj", "Demo.Restuarants.API.Models/"]
COPY ["Demo.Restuarants.Shared/Demo.Restuarants.Shared.csproj", "Demo.Restuarants.Shared/"]

RUN dotnet restore "Demo.Restuarants.Client.MVC/Demo.Restuarants.Client.MVC.csproj"

COPY . .
WORKDIR "/src/Demo.Restuarants.Client.MVC"
RUN dotnet build "Demo.Restuarants.Client.MVC.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Demo.Restuarants.Client.MVC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Demo.Restuarants.Client.MVC.dll"]
