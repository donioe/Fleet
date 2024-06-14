FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["source/Services/Fleet/Api/FleetApi/FleetApi.csproj", "source/Services/Fleet/Api/FleetApi/"]
RUN dotnet restore "source/Services/Fleet/Api/FleetApi/FleetApi.csproj"
COPY . .
WORKDIR "/src/source/Services/Fleet/Api/FleetApi"
RUN dotnet build "FleetApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FleetApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FleetApi.dll"]
