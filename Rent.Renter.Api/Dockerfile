﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8082
EXPOSE 8083

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Rent.Renter.Api/Rent.Renter.Api.csproj", "Rent.Renter.Api/"]
RUN dotnet restore "Rent.Renter.Api/Rent.Renter.Api.csproj"
COPY . .
WORKDIR "/src/Rent.Renter.Api"
RUN dotnet build "Rent.Renter.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Rent.Renter.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rent.Renter.Api.dll"]
