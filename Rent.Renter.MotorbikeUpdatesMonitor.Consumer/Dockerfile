#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Rent.Renter.MotorbikeUpdatesMonitor.Consumer/Rent.Renter.MotorbikeUpdatesMonitor.Consumer.csproj", "Rent.Renter.MotorbikeUpdatesMonitor.Consumer/"]
RUN dotnet restore "./Rent.Renter.MotorbikeUpdatesMonitor.Consumer/Rent.Renter.MotorbikeUpdatesMonitor.Consumer.csproj"
COPY . .
WORKDIR "/src/Rent.Renter.MotorbikeUpdatesMonitor.Consumer"
RUN dotnet build "./Rent.Renter.MotorbikeUpdatesMonitor.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Rent.Renter.MotorbikeUpdatesMonitor.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rent.Renter.MotorbikeUpdatesMonitor.Consumer.dll"]