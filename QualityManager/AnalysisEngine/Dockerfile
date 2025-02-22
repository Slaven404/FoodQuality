
#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Build faza (SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopiranje projektnog fajla
COPY ["AnalysisEngine/AnalysisEngine.csproj", "AnalysisEngine/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "AnalysisEngine/AnalysisEngine.csproj"

COPY AnalysisEngine/ AnalysisEngine/
COPY Shared/ Shared/

WORKDIR "/src/AnalysisEngine"

RUN dotnet build "AnalysisEngine.csproj" -c Release -o /app/build
RUN dotnet publish "AnalysisEngine.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AnalysisEngine.dll"]
