﻿FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["source/Services/Ship/Api/ShipApi/ShipApi.csproj", "source/Services/Ship/Api/ShipApi/"]
RUN dotnet restore "source/Services/Ship/Api/ShipApi/ShipApi.csproj"
COPY . .
WORKDIR "/src/source/Services/Ship/Api/ShipApi"
RUN dotnet build "ShipApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ShipApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShipApi.dll"]
